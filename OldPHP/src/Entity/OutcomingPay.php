<?php
namespace BlueIcons\FacebookInterfaceBundle\Entity;

/**
 * Model for pay comes from blueicons to user
 */
class OutcomingPay
{
    protected $timestamp;
    protected $payKey;    
    protected $receiverEmail;
    protected $incomingPay;
    protected $status;
    protected $isSended;
    /**
     * @var integer $id
     */
    private $id;


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
     * Set payKey
     *
     * @param string $payKey
     */
    public function setPayKey($payKey)
    {
        $this->payKey = $payKey;
    }

    /**
     * Get payKey
     *
     * @return string 
     */
    public function getPayKey()
    {
        return $this->payKey;
    }

    /**
     * Set receiverEmail
     *
     * @param string $receiverEmail
     */
    public function setReceiverEmail($receiverEmail)
    {
        $this->receiverEmail = $receiverEmail;
    }

    /**
     * Get receiverEmail
     *
     * @return string 
     */
    public function getReceiverEmail()
    {
        return $this->receiverEmail;
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
     * Set isSended
     *
     * @param integer $isSended
     */
    public function setIsSended($isSended)
    {
        $this->isSended = $isSended;
    }

    /**
     * Get isSended
     *
     * @return integer 
     */
    public function getIsSended()
    {
        return $this->isSended;
    }

    /**
     * Set incomingPay
     *
     * @param BlueIcons\FacebookInterfaceBundle\Entity\IncomingPay $incomingPay
     */
    public function setIncomingPay(\BlueIcons\FacebookInterfaceBundle\Entity\IncomingPay $incomingPay)
    {
        $this->incomingPay = $incomingPay;
    }

    /**
     * Get incomingPay
     *
     * @return BlueIcons\FacebookInterfaceBundle\Entity\IncomingPay 
     */
    public function getIncomingPay()
    {
        return $this->incomingPay;
    }
}