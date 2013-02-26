<?php
namespace BlueIcons\FacebookInterfaceBundle\Entity;

class Users
{
    protected $fb_user;
    /**
     * @var integer $id
     */
    private $id;

    /**
     * @var string $user
     */
    private $user;


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
     * Set user
     *
     * @param string $user
     */
    public function setUser($user)
    {
        $this->user = $user;
    }

    /**
     * Get user
     *
     * @return string 
     */
    public function getUser()
    {
        return $this->user;
    }
}