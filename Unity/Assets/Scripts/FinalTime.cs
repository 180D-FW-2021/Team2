using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalTime : MonoBehaviour
{
    //static Timer myTimer;
    public Text timeText;
    public float timeVal = 0;

    void OnEnable()
    {
        timeVal = PlayerPrefs.GetFloat("finalTime");
    }

    // Start is called before the first frame update
    void Start()
    {
        float timeToDisplay = timeVal;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
 
        timeText.text = "Time: " + string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
