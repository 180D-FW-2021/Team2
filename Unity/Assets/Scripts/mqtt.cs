﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System;

// Mqtt https://csharp.hotexamples.com/examples/uPLibrary.Networking.M2Mqtt/MqttClient/-/php-mqttclient-class-examples.html
// and RPi Rhythms

public class mqtt : MonoBehaviour
{
    public float forward;
    public bool left;
    public bool right;
    public bool jump;
    public bool duck;

    //create an instance of MqttClient class 
    private MqttClient client;

    static string myLog = ""; //intalize a log of messages on topic 

    // Start is called before the first frame update
    void Start()
    {
        //create MqttClient object
        // mqtt.eclipseprojects.io ip address
        // alternate test.mosquitto.org
        client = new MqttClient("mqtt.eclipseprojects.io");

        //When was the message published to the Broker
        client.MqttMsgPublished += client_MqttMsgPublished;

        //to be notified about recieved messages published on the subscribed topic
        client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;


        //call the Connect Method to connect to the broker
        string clientId = Guid.NewGuid().ToString();

        //connect
        client.Connect(clientId);

        // currently player will move forward/turn if message sent to topic/movement
        // strings from Raspberry Pi
        client.Subscribe(new string[] { "topic/movement" },
            new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });

        // currently player will jump/duck if message sent to topic/pose
        // strings from pose detection
        client.Subscribe(new string[] { "topic/pose" },
            new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });

        // initialize movement variables
        forward = 0;
        left = false;
        right = false;
        jump = false;
        duck = false;

    }

    void client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
    {
        Debug.Log("Subscribed for id = " + e.MessageId);
    }

    void client_MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
    {
        Debug.Log("inside client_MqttMsgPublished");
        Debug.Log("MessageId = " + e.MessageId + " Published = " + e.IsPublished);
        Debug.Log("MessageId = " + e.MessageId);
    }

    void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        //this function is called everytime you receive message
        //e.Message is a byte[]
        var str = System.Text.Encoding.UTF8.GetString(e.Message);
        myLog += str + "\n";

        Debug.Log("received a message");

        if (String.Equals(e.Topic, "topic/movement"))
        {
            Debug.Log(str);
            //Debug.Log(str == "Testing. Does this work?");
            if (str == "f")
            {
                forward = 1f;
            }
            if (str == "l")
            {
                left = true;
            }
            if (str == "r")
            {
                right = true;
            }
        }
        if (String.Equals(e.Topic, "topic/pose"))
        {
            Debug.Log(str);
            //Debug.Log(str == "Testing. Does this work?");
            if (str == "j")
            {
                jump = true;
            }
            if (str == "d")
            {
                duck = true;
            }
        }

    }

    public void resetMovementVars()
    {
        forward = 0;
        jump = false;
        duck = false;
    }

    public void resetPerspectiveVars()
    {
        left = false;
        right = false;
    }
}
