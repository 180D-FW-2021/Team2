using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

public class SettingsScripts : MonoBehaviour
{
    public Slider AudioSlider;
    public Toggle MovenetToggle;
    public Toggle HintsToggle;

    public RunMovenet myrunMovenet;

    private void Start()
    {
        string MovenetOn = PlayerPrefs.GetString("MovenetConnected");
        //UnityEngine.Debug.Log(MovenetOn);
        if (MovenetOn == "F")
        {
            MovenetToggle.isOn = false;
        }
    }

    public void changeAudio()
    {
        UnityEngine.Debug.Log("Audio volume changed");
        float LevelMusicVolume = AudioSlider.value;
        PlayerPrefs.SetFloat("LevelMusicVolume", LevelMusicVolume);
    }

    public void toggleMovenet()
    {
        if (MovenetToggle.isOn)
        {
            // launch Movenet
            myrunMovenet.StartMovenet();
        }
        else
        {
            // kill Movenet process
            UnityEngine.Debug.Log("Kill Movenet");
            foreach (Process p in Process.GetProcessesByName("position_tracking"))
            {
                p.CloseMainWindow();
            }
            PlayerPrefs.SetString("MovenetConnected", "F");
        }
    }

    public void toggleHints()
    {
        if (HintsToggle.isOn)
        {
            // turn on hints
            PlayerPrefs.SetInt("HintsOn", 1);
            UnityEngine.Debug.Log("Hints on");
        }
        else
        {
            // turn off hints
            PlayerPrefs.SetInt("HintsOn", 0);
            UnityEngine.Debug.Log("Hints off");
        }
    }
}
