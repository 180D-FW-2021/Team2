using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float timeVal = 0;
    public Text timeText;

    // Count up in time from 0 seconds
    void Update()
    {
        timeVal += Time.deltaTime;

        DisplayTime(timeVal);
    }

    // display minutes and seconds
    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        //float milliseconds = timeToDisplay % 1 * 1000;

        //timeText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void OnDisable()
    {
        Scene scene = SceneManager.GetActiveScene();
        PlayerPrefs.SetString("PrevScene", scene.name);
        PlayerPrefs.SetFloat("finalTime", timeVal);
    }
}
