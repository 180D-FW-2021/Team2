using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;


// Used to set playerprefs in settings menu and handle other settings
public class SettingsScripts : MonoBehaviour
{
    public Slider AudioSlider;
    public Toggle MovenetToggle;
    public Toggle HintsToggle;

    public RunMovenet myrunMovenet;

    private string playingAsGuest;

    private void Start()
    {
        // Initialize UI settings elements with current settings states
        if (PlayerPrefs.HasKey("MovenetConnected") && PlayerPrefs.GetString("MovenetConnected") == "F")
        {
            MovenetToggle.isOn = false;
        }
        if (PlayerPrefs.HasKey("LevelMusicVolume"))
        {
            AudioSlider.value = PlayerPrefs.GetFloat("LevelMusicVolume");
        }
        if (PlayerPrefs.HasKey("HintsOn"))
        {
            if (PlayerPrefs.GetInt("HintsOn") == 1)
            {
                HintsToggle.isOn = true;
            }
            else
            {
                HintsToggle.isOn = false;
            }

        }

        playingAsGuest = PlayerPrefs.GetString("PlayingAsGuest");

    }

    public void changeAudio()
    {
        UnityEngine.Debug.Log("Audio volume changed");
        float LevelMusicVolume = AudioSlider.value;
        PlayerPrefs.SetFloat("LevelMusicVolume", LevelMusicVolume);
    }

    public void toggleMovenet()
    {
        if (playingAsGuest == "F")
        {        
            if (MovenetToggle.isOn)
            {
                // launch Movenet
                myrunMovenet.StartMovenet();
                PlayerPrefs.SetString("MovenetConnected", "N");
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
        } else {
            if (MovenetToggle.isOn)
            {
                // can't toggle Movenet on. Turn it off
                MovenetToggle.isOn = false;
            }
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
