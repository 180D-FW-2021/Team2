using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    public float MinimumX = -90F;
    public float MaximumX = 90F;

    public Transform playerBody;

    float xRotation = 0f;

    private Quaternion m_CharacterTargetRot;
    private Quaternion m_CameraTargetRot;
    private Camera m_Camera;

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        m_CharacterTargetRot = transform.localRotation;
        m_CameraTargetRot = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        float yRot = Input.GetAxis("Mouse X") * XSensitivity * Time.deltaTime;
        float xRot = Input.GetAxis("Mouse Y") * YSensitivity * Time.deltaTime;

        //xRotation -= mouseY;
        //xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //playerBody.Rotate(Vector3.up * mouseX);

        float yRot1 = Input.GetKey("z") ? -YSensitivity : (Input.GetKey("x") ? YSensitivity : 0);

        //m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
        m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

        //m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

        //transform.localRotation = m_CharacterTargetRot;
        //transform.localRotation = m_CameraTargetRot;
        playerBody.Rotate(Vector3.up * yRot1);
    }

}
