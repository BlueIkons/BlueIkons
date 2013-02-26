<?php

namespace BlueIcons\FacebookInterfaceBundle\Tests\Controller;

use Symfony\Bundle\FrameworkBundle\Test\WebTestCase;

class DefaultControllerTest extends WebTestCase
{
    public function testIndex()
    {
        $client = static::createClient();

        $crawler = $client->request('GET', '/facebook/');
        
        //$this->assertEquals(200,$client->getResponse()->getStatusCode(),"HTTP 200");
        
        //print_r($crawler);
        print_r($client->getResponse()->getContent());
        //$this->assertTrue($crawler->filter('html:contains("New gift")')->count() > 0);
        
    }
    
    public function testPaypalForm() 
    {
        //$client = static::createClient();
        //$crawler = $client->request('GET', '/facebook/form/');
    }
}
