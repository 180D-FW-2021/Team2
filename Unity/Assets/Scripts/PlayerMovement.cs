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

public class PlayerMovement : MonoBehaviour
{
    //create an instance of MqttClient class 
    //private MqttClient client;
    public mqtt myMQTT;

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
    
    // Start is called before the first frame update
    void Start()
    {
        // initialize ducking variables
        cylinderHeight = cylinder.transform.localScale;
        ducked = false;
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
        float z = -(Input.GetAxis("Vertical") + myMQTT.forward);

        // move player forward/backward/left/right
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // player jump
        if ((Input.GetButtonDown("Jump") || myMQTT.jump) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        // player duck
        if(Input.GetKey("p") || myMQTT.duck)
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

        myMQTT.resetMovementVars();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.tag == "EndObject")
        {
            Debug.Log("Hit something");
            GameManager.Instance.UpdateGameState(GameState.Victory);
        }
    }
}
