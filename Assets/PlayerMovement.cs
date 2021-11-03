using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;

using System;

//reference https://www.youtube.com/watch?v=_QajrabyTJc&list=RDCMUCYbK_tjZ2OrIZFBvU6CCMiA&index=17
// Mqtt https://csharp.hotexamples.com/examples/uPLibrary.Networking.M2Mqtt/MqttClient/-/php-mqttclient-class-examples.html
// and RPi Rhythms

public class PlayerMovement : MonoBehaviour
{
    //create an instance of MqttClient class 
    private MqttClient client;

    static string myLog = ""; //intalize a log of messages on topic 

    public CharacterController controller;

    public float speed = 8f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    
    // Start is called before the first frame update
    void Start()
    {
        //create MqttClient object
        //client = new MqttClient(IPAddress.Parse("131.179.8.132"), 1883, false, null);
        client = new MqttClient("137.135.83.217");

        //When was the message published to the Broker
        client.MqttMsgPublished += client_MqttMsgPublished;

        //to be notified about recieved messages published on the subscribed topic
        client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;


        //call the Connect Method to connect to the broker
        string clientId = Guid.NewGuid().ToString();

        //connect
        client.Connect(clientId);

        // subscribe to the topic "topic" with QoS 0, can subscribe to any number of topics 
        //adding red
        client.Subscribe(new string[] { "topic/red" },
            new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
        //adding gesture
        client.Subscribe(new string[] { "topic/gesture" },
            new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = -Input.GetAxis("Horizontal");
        float z = -Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
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
        bool red = false;
        bool gesture = false;
        //this function is called everytime you receive message
        //e.Message is a byte[]
        var str = System.Text.Encoding.UTF8.GetString(e.Message);
        myLog += str + "\n";

        Debug.Log("received a message");

        if (String.Equals(e.Topic, "topic/red"))
        {
            //you receive message for red color
            red = true;
            Debug.Log("red is true");
        }
        if (String.Equals(e.Topic, "topic/gesture"))
        {
            //you receive message for gesture
            gesture = true;
            Debug.Log("gesture is true");
        }

        Debug.Log("entering if statement");

        if (red == true)
        {
            Debug.Log("red is true");
        }

        if (gesture == true)
        {
            Debug.Log("gesture_pred is ");
        }
    }
}
