<?php
namespace BlueIcons\FacebookInterfaceBundle\Entity;

class PreIncoming
{
    protected $fb_receiver_id;
    protected $message;
    protected $item;
    protected $token;
    /**
     * @var integer $id
     */
    private $id;

    /**
     * @var string $receiverId
     */
    private $receiverId;


    /**
     * Get id
     *
     * @return integer 
     */
    public function getId()
    {
        return $this->id;
    }

    /**
     * Set token
     *
     * @param string $token
     */
    public function setToken($token)
    {
        $this->token = $token;
    }

    /**
     * Get token
     *
     * @return string 
     */
    public function getToken()
    {
        return $this->token;
    }

    /**
     * Set receiverId
     *
     * @param string $receiverId
     */
    public function setReceiverId($receiverId)
    {
        $this->receiverId = $receiverId;
    }

    /**
     * Get receiverId
     *
     * @return string 
     */
    public function getReceiverId()
    {
        return $this->receiverId;
    }

    /**
     * Set message
     *
     * @param string $message
     */
    public function setMessage($message)
    {
        $this->message = $message;
    }

    /**
     * Get message
     *
     * @return string 
     */
    public function getMessage()
    {
        return $this->message;
    }

    /**
     * Set item
     *
     * @param BlueIcons\FacebookInterfaceBundle\Entity\Items $item
     */
    public function setItem(\BlueIcons\FacebookInterfaceBundle\Entity\Items $item)
    {
        $this->item = $item;
    }

    /**
     * Get item
     *
     * @return BlueIcons\FacebookInterfaceBundle\Entity\Items 
     */
    public function getItem()
    {
        return $this->item;
    }
}