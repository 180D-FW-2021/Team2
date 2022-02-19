using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System;

public class LoadingMovenet : MonoBehaviour
{
    // Public: accessible from other scripts
    // Static: don't want to reference this specific PauseMenu script
    // Just want to check from other scripts whether game is paused
    // public static bool IsGamePaused = false;
    // in other scripts, can reference this as if (PauseMenu.IsGamePaused)
    public GameObject LoadingMovenetUI;

    public GameManager GameManagerScript;

    public bool connectedToMqtt;

    // FROM mqtt.cs:
    //create an instance of MqttClient class 
    private MqttClient client;
    private string username;

// Start is called before the first frame update
    void Start()
    {
        connectedToMqtt = false; 

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


    void Awake() {
        GameManager.OnGameStateChanged += GameManagerOnOnGameStateChanged;
        GameManagerScript = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
    }

    void OnDestroy() {
        GameManager.OnGameStateChanged -= GameManagerOnOnGameStateChanged;
    }

    void Update() {
        if (connectedToMqtt == true) {
            GameManagerScript.UpdateGameState(GameState.LoadSelectedLevel);
        }
    }

    private void GameManagerOnOnGameStateChanged(GameState state) {
        if (state == GameState.WaitForMovenet) {
            DisplayPopUp();
        }
        // } else if (state == GameState.Resumed) {
        //     Resume();
        // }
        
    }

    // Public functions to trigger from buttons
    // public void Resume() 
    // {
    //     pauseMenuUI.SetActive(false); // Set UI to not active
    //     Time.timeScale = 1f;  // Set time to normal rate
    //     GameManager.Instance.UpdateGameState(GameState.Playing);
    // }

    public void DisplayPopUp() 
    {
        LoadingMovenetUI.SetActive(true); // Set game object to active
        // Time.timeScale = 0f;  // completely stop time
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
            Debug.Log(str);
            connectedToMqtt = true;
            //Debug.Log(str == "Testing. Does this work?");
        }
    }

}
