﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// based on code here: https://forum.unity.com/threads/best-way-to-get-variable-from-another-script-c-unity.579322/

public class DisplayWarnings : MonoBehaviour
{

    public GameObject warningUI;

    public GameObject objectContainingMqtt;
    private mqtt mqtt_script;

    // Start is called before the first frame update
    void Start()
    {
        // Obtain reference to mqtt script
        mqtt_script = objectContainingMqtt.GetComponent<mqtt>();
    }

    // Update is called once per frame
    void Update()
    {
        // Use flag variable to set warning message on/off
        if (mqtt_script.outofframe == true && warningUI.activeSelf == false) {
            warningUI.SetActive(true);
        } else if (mqtt_script.outofframe == false && warningUI.activeSelf == true){
            warningUI.SetActive(false);
        }

    }
}
