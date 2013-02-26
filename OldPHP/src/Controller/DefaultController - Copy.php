<?php

namespace BlueIcons\FacebookInterfaceBundle\Controller;

use Symfony\Bundle\FrameworkBundle\Controller\Controller;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\HttpFoundation\Response;
use BlueIcons\FacebookInterfaceBundle\Entity\Users;
use BlueIcons\FacebookInterfaceBundle\Entity\IncomingPay;
use BlueIcons\FacebookInterfaceBundle\Entity\OutcomingPay;
use BlueIcons\FacebookInterfaceBundle\Entity\PreIncoming;


class DefaultController extends Controller
{   
    public static $ADMIN_USERS = array('100001916820536', '1566199117', '100000069943264', '100003369248015', '100001688981096'); //Sasha, Sarah, Pasha, Anya, Misha
    /**
     *  
     * @return type 
     */
    public function indexAction()
    {   
        $fb = $this->get('facebook');
        $userId = $fb->getUser();
	$router = $this->get('router');
        $url = $router->generate('BlueIconsFI_form');
        if (!$userId) {
            $baseUrl = $this->getBaseUrl($this->container->getParameter('facebook.bi_server_path'));
            $url = $fb->getLoginUrl(array('scope' => 'publish_stream', 'redirect_uri' => $baseUrl.'auth_redirect/?path=form'));
        } else {
            $url .= '?token='.$fb->getAccessToken();
        }
        
        if (in_array($userId, self::$ADMIN_USERS)) {
            $isAdmin = true;
        } else {
            $isAdmin = false;
        }
        
        return $this->render('BlueIconsFacebookInterfaceBundle:Default:index.html.twig', array('url' => $url, 'isAdmin' => $isAdmin));
    }
    
    
    /**
     * Main controller for gift sending
     * 
     * @param Request $request
     * @return type 
     */
    public function paypalFormAction(Request $request)
    {   
        try {
            $fb = $this->get('facebook');
            $token = $request->get('token');
            if ($token) {
                $fb->setAccessToken($token);
            }

            $userId = $fb->getUser();

            if (in_array($userId, self::$ADMIN_USERS)) {
                $isAdmin = true;
            } else {
                $isAdmin = false;
            }
            
            if ($request->getMethod() == 'POST' && $request->get('item', '')) {
                return $this->proceedPaypalSend($request);            
            } else {
                $me = $fb->api('/me');
                $items = $this->getDoctrine()->getRepository('BlueIconsFacebookInterfaceBundle:Items')->findAll();
		$friend = null;
		if ($friendId = $request->get('id')) {
	            $friend = $fb->api('/'.$friendId.'?fields=id,name,picture');
		}

                return $this->render('BlueIconsFacebookInterfaceBundle:Default:paypalForm.html.twig', array('items' => $items, 'name' => $me['name'], 'friend' => $friend, 'isAdmin' => $isAdmin));
            }
        } catch(\Facebook\FacebookApiException $e) {
            return $this->authenticateApp('form');
        }
    }
    

    
    /**
     * Initiate Paypal digital goods excpress checkout transaction and save info about gift to PreIncoming table
     * 
     * @param Request $request
     * @return type 
     */
    private function proceedPaypalSend(Request $request)
    {
        $pp = $this->get('paypal');
        
        $selectedItemId = $request->get('item');
        $item = $this->getDoctrine()->getRepository('BlueIconsFacebookInterfaceBundle:Items')->find($selectedItemId);
        
        $ppResponse = $pp->setExpressCheckout($item->getAmount());
        
        //@TODO  check if transfer was successfull
        $this->savePreIncomingData($request, $ppResponse);
        
        return $this->render('BlueIconsFacebookInterfaceBundle:Default:paypal_user_auth.html.twig', array(
            'token' => $pp->getOption('token'), 
        ));
    }
    
    /**
     * Save parameters of gift, it allows us not to send this parameters though paypal actions
     * 
     * @param Request $request
     * @param type $ppResponse 
     */
    private function savePreIncomingData(Request $request, $ppResponse)
    {
        $tmp = new PreIncoming();
        
        $selectedItemId = $request->get('item');
        $item = $this->getDoctrine()->getRepository('BlueIconsFacebookInterfaceBundle:Items')->find($selectedItemId);
        $tmp->setItem($item);
        
        $tmp->setMessage(htmlentities($request->get('message')));
        
        $matches = array();
        preg_match('/TOKEN=([\-\w]*)&/', $ppResponse, $matches);
        $tmp->setToken($matches[1]);
        
        $tmp->setReceiverId($request->get('friend'));
        //$tmp->setReceiverId('100001916820536');
        
        $em = $this->getDoctrine()->getEntityManager();
        $em->persist($tmp);
        $em->flush();
    }
    
    /**
     * Returns view that redirects to special paypal login form
     * 
     * @param Request $request
     * @return Response
     */
    public function confirmGiftSendAction(Request $request)
    {
        return $this->render('BlueIconsFacebookInterfaceBundle:Default:confirm_gift_send.html.twig',  array(
            'token' => $request->get('token'),
            'payerId' => $request->get('PayerID'),
        ));
    }
    
    public function rejectGiftSendAction(Request $request) 
    {
        return $this->render('BlueIconsFacebookInterfaceBundle:Default:reject_gift_send.html.twig');
    }
    
    /**
     * Do EC payment, save result to outcoming_pay and returns json with corellation id and recipient name
     * 
     * @param Request $request
     * @return Response
     */
    public function successGiftSendAction($token = null, $payerId = null)
    {
        $fb = $this->get('facebook');        
        $pp = $this->get('paypal');
        $pp->cleanData();
        
        if ($token && $payerId) {
            $pp->setData('TOKEN', $token);
            $pp->setData('PAYERID', $payerId);
        }
        
        $tmp = $this->getDoctrine()->getRepository('BlueIconsFacebookInterfaceBundle:PreIncoming')->findOneBy(array('token' => $token));
        
        $ppResponse = $pp->doExpressCheckoutPayment($tmp->getItem()->getAmount());
        $incoming = $this->saveConfirmationData($token, $ppResponse);
        
        $friend = $fb->api('/'.$incoming->getReceiverId());
        $me = $fb->api('/me');
        $receiver = '/'.$friend['id'].'/feed';
        $message =  'You have received a gift from '.$me['name'].' using BlueIkons';
        if ($incoming->getMessage() != '') {
            $message .= ', with message: '.$incoming->getMessage();
        }
        try {
            $fbResponse = $fb->api($receiver, 'POST', array(
                'link' => 'http://apps.facebook.com/'.$fb->getAppId().'/getgift/'.$incoming->getGiftHash().'/',
                'name' => 'BlueIkons on Facebook | Click to accept the gift',
                'message' => $message,
                'picture' => str_replace('http://', 'https://', $this->getBaseUrl('web')) . '/bundles/blueiconsfacebookinterface/images/'.$incoming->getItem()->getIconPath(),
                //'picture' => $this->getBaseUrl($this->container->getParameter('facebook.bi_server_path')).'/bundles/blueiconsfacebookinterface/images/'.$incoming->getItem()->getIconPath(),
                'description' => 'Faster than a Gift Card... and more fun!',
            ));
        } catch(\Facebook\FacebookApiException $e) {}

        $response = array(
            'corellationId' => $incoming->getCorellationId(),
            'friendName' => $friend['name'],
        );
        return new Response(json_encode($response));
    }
    
    /**
     *  Saves info about sended gift to database
     * 
     * @param string $token paypal token
     * @param type $response paypal respons, contains transaction result data
     * @return IncomingPay 
     */
    private function saveConfirmationData($token, $response)
    {    
        $fb = $this->get('facebook');
        $me = $fb->api('/me');
        $user = $this->getDoctrine()->getRepository('BlueIconsFacebookInterfaceBundle:Users')->findOneBy(array('user' => $me['id']));
        $incoming = new IncomingPay();
        
        $tmp = $this->getDoctrine()->getRepository('BlueIconsFacebookInterfaceBundle:PreIncoming')->findOneBy(array('token' => $token));

        $incoming->setUser($user); // user
        $incoming->setReceiverId($tmp->getReceiverId()); // reseiver
        $incoming->setStatus('Pending'); // status
        $incoming->setMessage($tmp->getMessage()); //message
        $incoming->setToken($tmp->getToken()); //token
        
        $matches = array();
        preg_match('/CORRELATIONID=(\w*)&/', $response, $matches);
        $incoming->setCorellationId($matches[1]); // corellation
        
        preg_match('/TIMESTAMP=([0-9\-:TZ]*)&/', $response, $matches);
        $timestamp = $matches[1];
        $timestamp = str_replace('T', ' ', $timestamp);
        $timestamp = str_replace('Z', '', $timestamp);
        $date = new \DateTime($timestamp);
        $incoming->setTimestamp($date); // timestamp

        preg_match('/PAYMENTINFO_0_AMT=([\.\d]*)&/', $response, $matches);
        $incoming->setAmount($matches[1]); //amount
        
        $incoming->setItem($tmp->getItem());
        
        $incoming->setGiftHash(md5(uniqid(rand(), true)));

        $em = $this->getDoctrine()->getEntityManager();
        $em->persist($incoming);
        $em->flush();
        
        return $incoming;
    }
    
    /**
     * Transfers money to recipient paypal account and save info about transaction result
     * 
     * @param Request $request
     * @return Response 
     */
    //@TODO сделать роут до getgift/
    public function getGiftAction(Request $request)
    {
        try {
            //@FIXME проверить ситуацию, когда 2 подарка одинаковой суммы и от одного и того же адресата
            
            $giftHash = $request->get('giftHash', '');
            $fb = $this->get('facebook');
            $userId = $fb->getUser();
            if (in_array($userId, self::$ADMIN_USERS)) {
                $isAdmin = true;
            } else {
                $isAdmin = false;
            }
            
            $me = $fb->api('/me');
            $em = $this->getDoctrine()->getEntityManager();
	    if ($giftHash != '') {
                $incoming = $em->getRepository('BlueIconsFacebookInterfaceBundle:IncomingPay')->findOneBy(array('giftHash' => $giftHash));
	    } else {
		$incoming = null;
	    }
            
            if ($incoming) {
                $item = $incoming->getItem();
                if ($request->getMethod() == 'POST' && $request->get('email')) {
                    $email = $request->get('email');
		    
                    //$pp = $this->get('paypal');
                    //$response = $pp->doPay($email, $incoming->getItem()->getAmount());
                    //$outcoming = $this->finishGiftSend($incoming, $response, $email);
                    
                    
                    if (filter_var($email, FILTER_VALIDATE_EMAIL) && $incoming->getStatus() == 'Pending') {
                        $outcoming = $this->finishGiftSendStub($incoming, '', $email);
                        $response = array('paykey' => $outcoming->getPayKey());
                        return new Response(json_encode($response));
                    } else {
                        $response = array('paykey' => 'fail');
                        return new Response(json_encode($response));
                    }
                }
                
                $sender = $fb->api('/'.$incoming->getUser()->getUser());
                
                $isReceived = true;
                if ($me['id'] == $incoming->getReceiverId()) {
                    $isReceiver = true;
                    if ($incoming->getStatus() == 'Pending') {
                        $isReceived = false;
                    }
                } else {
                    $receiver = $fb->api('/'.$incoming->getReceiverId().'?fields=name');
                    $sender['receiver'] = $receiver['name'];
                }
            } else {
                return $this->render('BlueIconsFacebookInterfaceBundle:Default:no_such_gift.html.twig');
            }

            return $this->render('BlueIconsFacebookInterfaceBundle:Default:transfer_recipient.html.twig', array('sender' => $sender, 'isReceiver' => $isReceiver, 'isReceived' => $isReceived, 'item' => $item, 'isAdmin' => $isAdmin));
        } catch (\Facebook\FacebookApiException $e) {
            $trace = $e->getTrace();
            $path = 'getgift';
            $giftHash = $trace[4]['args'][0]->get('giftHash');
            if ($giftHash != '') {
                $path .= '/'.$giftHash;
            }
            return $this->authenticateApp($path);
        }
    }
    
    /**
     * Saves transaction info about received money (status takes from PayPal response)
     * 
     * @param IncomingPay $incoming
     * @param string $response
     * @param string $email
     * @return OutcomingPay 
     */
    private function finishGiftSend(IncomingPay $incoming, $response, $email)
    {
        $outcoming = new OutcomingPay();
        $outcoming->setIncomingPay($incoming);
        $outcoming->setPayKey($response['payKey']);
        $outcoming->setReceiverEmail($email);
        $outcoming->setStatus($response['paymentExecStatus']);
        
        $timestamp = $response['responseEnvelope.timestamp'];
        $timestamp = str_replace('T', ' ', $timestamp);
        $timestamp = str_replace('Z', '', $timestamp);
        $date = new \DateTime($timestamp);
        $outcoming->setTimestamp($date);
        
        $em = $this->getDoctrine()->getEntityManager();
        if ($outcoming->getStatus() == 'COMPLETED') {
            $incoming->setStatus($outcoming->getStatus());
        }

        $em->persist($outcoming);
        $em->flush();
        
        return $outcoming;
    }
    
    private function finishGiftSendStub(IncomingPay $incoming, $response, $email) {
        $outcoming = new OutcomingPay();
        $outcoming->setIncomingPay($incoming);
        $outcoming->setPayKey($incoming->getCorellationId());
        if (preg_match)
        $outcoming->setReceiverEmail($email);
        $outcoming->setStatus('COMPLETED');
        
        $timestamp = $response['responseEnvelope.timestamp'];
        $timestamp = str_replace('T', ' ', $timestamp);
        $timestamp = str_replace('Z', '', $timestamp);
        $date = new \DateTime(date('Y-m-d H:i:s'));
        $outcoming->setTimestamp($date);
        
        $em = $this->getDoctrine()->getEntityManager();
        if ($outcoming->getStatus() == 'COMPLETED') {
            $incoming->setStatus($outcoming->getStatus());
        }
        
        $outcoming->setIsSended('0');

        $em->persist($outcoming);
        $em->flush();
        
        return $outcoming;
    }

    /**
     * Returns list of receivet gifts
     * 
     * @return Response 
     */
    public function giftsListAction()
    {
        try {
            $fb = $this->get('facebook');
            $userId = $fb->getUser();
            
            if (in_array($userId, self::$ADMIN_USERS)) {
                $isAdmin = true;
            } else {
                $isAdmin = false;
            }
            
            $me = $fb->api('/me');
            $gifts = array();
            $giftsToReceive = array();
            $incomings = $this->getDoctrine()->getRepository('BlueIconsFacebookInterfaceBundle:IncomingPay')->findBy(array('receiverId' => $me['id']));
            if ($incomings) {
                    $fbFriends = $fb->api('/me/friends?fields=id,name,picture');
                    $friends = array();
                    foreach($fbFriends['data'] as $friend) {
                            $friends[$friend['id']]['name'] = $friend['name'];
                            $friends[$friend['id']]['picture'] = $friend['picture'];
                    }

                    foreach ($incomings as $incoming) {
                            $out = $this->getDoctrine()->getRepository('BlueIconsFacebookInterfaceBundle:OutcomingPay')->findOneBy(array('incomingPay' => $incoming->getId()));
                            
                            $id = $incoming->getUser()->getUser();
                            if (isset($friends[$id])) {
                                $friend = $friends[$id]['name'];
                                $photo = $friends[$id]['picture'];
                            } else {
                                $friend = 'Unknown friend';
                                $photo = '';
                            }
                            
                            $gift = array(
                                'sender' => $friend,
                                'senderPhoto' => $photo,
                                'item' => array(
                                    'name' => $incoming->getItem()->getName(),
                                    'iconPath' => $incoming->getItem()->getIconPath(),
                                    'amount' => $incoming->getItem()->getAmount(),
                                ),
                                'hash' => $incoming->getGiftHash(),
                                'message' => $incoming->getMessage(),
                            );
                            if ($out) {
                                $gifts[] = $gift;
                            } else {
                                $giftsToReceive[] = $gift;
                            }
                    }
            }

            return $this->render('BlueIconsFacebookInterfaceBundle:Default:gifts_list.html.twig', array('gifts' => $gifts, 'giftsToReceive' => $giftsToReceive, 'isAdmin' => $isAdmin));
        } catch(\Facebook\FacebookApiException $e) {         
            return $this->authenticateApp('list');
        }
    }
    
    public function showTermsAction(Request $request)
    {
        //@TODO find better alternative
        $docNum = $request->get('term');
        switch($docNum) {
            case '1': 
                return $this->render('BlueIconsFacebookInterfaceBundle:Default:terms_privacy.html.twig');
            case '2':
                return $this->render('BlueIconsFacebookInterfaceBundle:Default:terms_T&C.html.twig');
            default:
                return new Response('No such terms');
        }
    }
    
    /**
     * Redirects user when hi allowed app and save his facebook id
     * 
     * @param Request $request
     * @return Response
     */
    public function authRedirectAction(Request $request)
    {
        $fb = $this->get('facebook');
        $userId = $fb->getUser();
        $user = $this->getDoctrine()->getRepository('BlueIconsFacebookInterfaceBundle:Users')->findOneBy(array('user' => $userId));
        if (!$user) {
            $em = $this->getDoctrine()->getEntityManager();
            $new_user = new Users();
            $new_user->setUser($userId);
            $em->persist($new_user);
            $em->flush();
        }
        
        $fbConfig = $this->container->getParameter('facebook.config');
        $url = $this->getProtocol().'://'.'apps.facebook.com'.'/'.$fbConfig['namespace'].'/';
        if ($path = $request->get('path', '')) {
            $url .= $path.'/';
        }
        
        return $this->render('BlueIconsFacebookInterfaceBundle:Default:auth_redirect.html.twig', array('url' => $url));
    }
    
    private function authenticateApp($returnPath) {
        $fb = $this->get('facebook');
        $baseUrl = $this->getBaseUrl($this->container->getParameter('facebook.bi_server_path'));
        $url = $baseUrl.'auth_redirect/?path='.$returnPath;
        $fbUrl = $fb->getLoginUrl(array('scope' => 'publish_stream', 'redirect_uri' => $url));

        return $this->render('BlueIconsFacebookInterfaceBundle:Default:app_auth.html.twig', array('fbUrl' => $fbUrl));  
    }
    
    /**
     * Autocompletion part in friend selection
     * 
     * @param Request $request
     * @return Response 
     */
    public function searchFriendAction(Request $request) 
    {
        //@TODO make transliteration of names
        $name = urldecode($request->get('term'));
        $fb = $this->get('facebook');
        $friends = $fb->api('/me/friends?fields=id,name,picture');

        $result = array();
        
        foreach($friends['data'] as $friend) {
            if (mb_strpos(mb_strtolower($friend['name']), mb_strtolower($name)) !== false) {
                array_push($result, array("value"=> $friend['name'], "label" => $friend['picture'], "id" => $friend['id']));
            }
        }
        
        $json = json_encode($result);
        return new Response($json);
    }
    
    public function ManualSendGiftToRecipientAction(Request $request)
    {   
        try {
            $fb = $this->get('facebook');

            $user_id = $fb->getUser();

            if (!$user_id) {
                throw new \Facebook\FacebookApiException();
            }

            if (in_array($user_id, self::$ADMIN_USERS)) {
                if ($request->get('sended', '')) {
                    $sended = $request->get('sended');
                    $outcoming = $this->getDoctrine()->getRepository('BlueIconsFacebookInterfaceBundle:OutcomingPay')->find($sended);
                    $outcoming->setIsSended('1');
                    $em = $this->getDoctrine()->getEntityManager();
                    $em->flush();
                }
                $me = $fb->api('/me');
                $outcomings = $this->getDoctrine()->getRepository('BlueIconsFacebookInterfaceBundle:OutcomingPay')->findBy(array('isSended' => 0));

                return $this->render('BlueIconsFacebookInterfaceBundle:Default:manual_send_gift_to_recipient.html.twig', array('outcomings' => $outcomings));
                
            } else {
                return new Response('You are not allowed to be here!');
            }
        } catch(\Facebook\FacebookApiException $e) {         
            return $this->authenticateApp('admin');
        }
    }
    
    private function getBaseUrl($path = null) {
        $protocol = $this->getProtocol();
        $url = $protocol.'://'.$this->getRequest()->getHttpHost().'/';
        if ($path) {
            $url .= $path.'/';
        }
        return $url;
    }
    
    private function getProtocol()
    {
        if ($this->getRequest()->isSecure()) {
            $protocol = 'https';
        } else {
            $protocol = 'http';
        }
        return $protocol;
    }
    
}
