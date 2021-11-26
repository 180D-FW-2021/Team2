using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public mqtt myMQTT;

    public float YSensitivity = 1f;

    public Transform playerBody;

    void Update()
    {
        // rotate player left with "z" and right with "x"
        float yRot1 = Input.GetKey("z") ? -YSensitivity : (Input.GetKey("x") ? YSensitivity : 0);

        yRot1 += myMQTT.left ? -YSensitivity : (myMQTT.right ? YSensitivity : 0);

        playerBody.Rotate(Vector3.up * yRot1);
    }

}
