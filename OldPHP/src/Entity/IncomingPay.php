<?php
namespace BlueIcons\FacebookInterfaceBundle\Entity;
/**
 * Model for pay comes from user to blueIcons
 */
class IncomingPay
{
    protected $id;
    protected $user;
    protected $timestamp;
    protected $corellationId;
    protected $token;    
    protected $receiverId;
    protected $amount;
    protected $item;
    protected $status;
    protected $message;
    protected $giftHash;

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
     * Set timestamp
     *
     * @param datetime $timestamp
     */
    public function setTimestamp($timestamp)
    {
        $this->timestamp = $timestamp;
    }

    /**
     * Get timestamp
     *
     * @return datetime 
     */
    public function getTimestamp()
    {
        return $this->timestamp;
    }

    /**
     * Set corellationId
     *
     * @param string $corellationId
     */
    public function setCorellationId($corellationId)
    {
        $this->corellationId = $corellationId;
    }

    /**
     * Get corellationId
     *
     * @return string 
     */
    public function getCorellationId()
    {
        return $this->corellationId;
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
     * Set amount
     *
     * @param integer $amount
     */
    public function setAmount($amount)
    {
        $this->amount = $amount;
    }

    /**
     * Get amount
     *
     * @return integer 
     */
    public function getAmount()
    {
        return $this->amount;
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
     * Set status
     *
     * @param string $status
     */
    public function setStatus($status)
    {
        $this->status = $status;
    }

    /**
     * Get status
     *
     * @return string 
     */
    public function getStatus()
    {
        return $this->status;
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
     * Set giftHash
     *
     * @param string $giftHash
     */
    public function setGiftHash($giftHash)
    {
        $this->giftHash = $giftHash;
    }

    /**
     * Get giftHash
     *
     * @return string 
     */
    public function getGiftHash()
    {
        return $this->giftHash;
    }

    /**
     * Set user
     *
     * @param BlueIcons\FacebookInterfaceBundle\Entity\Users $user
     */
    public function setUser(\BlueIcons\FacebookInterfaceBundle\Entity\Users $user)
    {
        $this->user = $user;
    }

    /**
     * Get user
     *
     * @return BlueIcons\FacebookInterfaceBundle\Entity\Users 
     */
    public function getUser()
    {
        return $this->user;
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