using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Server myServer;

    public float YSensitivity = 1f;

    public Transform playerBody;

    void Update()
    {
        //Debug.Log(GameManager.Instance.State);
        if (GameManager.Instance.State != GameState.Paused) {
            // rotate player left with "z" and right with "x"
            float yRot1 = Input.GetKey("z") ? -YSensitivity : (Input.GetKey("x") ? YSensitivity : 0);

            yRot1 += myServer.left ? -(YSensitivity * 0.3f) : (myServer.right ? (YSensitivity * 0.3f) : 0);

            playerBody.Rotate(Vector3.up * yRot1);
        }
    }

}
