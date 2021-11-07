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

    public GameObject cylinder;
    private Vector3 scaleChange, positionChange;
    private Vector3 cylinderHeight;
    private bool ducked;

    public float speed = 8f;
    public float gravity = -19.62f;
    public float jumpHeight = 1f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    private float forward;
    
    // Start is called before the first frame update
    void Start()
    {
        //create MqttClient object
        // mqtt.eclipseprojects.io ip address
        client = new MqttClient("137.135.83.217");

        //When was the message published to the Broker
        client.MqttMsgPublished += client_MqttMsgPublished;

        //to be notified about recieved messages published on the subscribed topic
        client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;


        //call the Connect Method to connect to the broker
        string clientId = Guid.NewGuid().ToString();

        //connect
        client.Connect(clientId);

        // currently player will move forward if message sent to topic/red
        client.Subscribe(new string[] { "topic/red" },
            new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });

        // initialize ducking variables
        cylinderHeight = cylinder.transform.localScale;
        ducked = false;
        forward = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        // Checks that player is currently on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Used to set velocity back to 0 after player has jumped and landed on the ground
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // inputs from arrow keys or "WASD"
        float x = -Input.GetAxis("Horizontal");
        float z = -(Input.GetAxis("Vertical") + forward);
        forward = 0;

        // move player forward/backward/left/right
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // player jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        // player duck
        if(Input.GetKey("p"))
        {
            if (!ducked) {
                cylinder.transform.localScale -= new Vector3(0, 0.6f, 0);
            }
            ducked = true;
        }
        else
        {
            cylinder.transform.localScale = cylinderHeight;
            ducked = false;
        }
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

        if (String.Equals(e.Topic, "topic/red"))
        {
            Debug.Log("red is true");
            Debug.Log(str);
            //Debug.Log(str == "Testing. Does this work?");
            forward = 1f;
        }

    }
}
