using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System;

// Check whether Movenet is connected for the first time
public class mqtt_MovenetConnected : MonoBehaviour
{
    // Note whether we are connected to Movenet
    private string connectedToMqtt;

    // FROM mqtt.cs:
    //create an instance of MqttClient class 
    private MqttClient client;
    private string username;

    // Start is called before the first frame update
    void Start()
    {
        connectedToMqtt = PlayerPrefs.GetString("MovenetConnected", "F");

        if (connectedToMqtt == "F") {
            // Obtain user information
            username = PlayerPrefs.GetString("Username");
            Debug.Log("mqtt " + username);

            //create MqttClient object
            // mqtt.eclipseprojects.io ip address
            // alternate test.mosquitto.org
            client = new MqttClient("test.mosquitto.org");

            //When was the message published to the Broker
            client.MqttMsgPublished += client_MqttMsgPublished;

            //to be notified about recieved messages published on the subscribed topic
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;


            //call the Connect Method to connect to the broker
            string clientId = Guid.NewGuid().ToString();

            //connect
            client.Connect(clientId);

            // currently player will jump/duck if message sent to topic/pose
            // strings from pose detection
            client.Subscribe(new string[] { "topic/pose/" + username },
                new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });

        }
    }

    void Update()
    {
        if (connectedToMqtt == "T") {
            Debug.Log("Updating connection status to Mqtt");
            PlayerPrefs.SetString("MovenetConnected", "T");
            connectedToMqtt = "F";
        }
    }

    void OnDestroy() {
        if (client != null && client.IsConnected)
            client.Disconnect();
    }

    // FROM mqtt.cs:
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

        Debug.Log("received message from movenet ready to load level");

        if (String.Equals(e.Topic, "topic/pose/" + username))
        {
            if (str == "g" || str == "j" || str == "d" || str == "s") {
                Debug.Log(str);
                connectedToMqtt = "T";
            }
            //Debug.Log(str == "Testing. Does this work?");
        }
    }


}
