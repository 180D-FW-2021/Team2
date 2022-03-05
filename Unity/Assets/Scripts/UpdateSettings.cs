using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

public class UpdateSettings : MonoBehaviour
{
    public AudioSource gameMusic;
    public GameObject trailObject;

    public Slider AudioSlider;
    public Toggle MovenetToggle;
    public Toggle HintsToggle;

    public RunMovenet myrunMovenet;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("LevelMusicVolume"))
        {
            gameMusic.volume = PlayerPrefs.GetFloat("LevelMusicVolume");
            AudioSlider.value = PlayerPrefs.GetFloat("LevelMusicVolume");
        }
        if (PlayerPrefs.HasKey("MovenetConnected") && PlayerPrefs.GetString("MovenetConnected") == "F")
        {
            MovenetToggle.isOn = false;
        }
        if (PlayerPrefs.HasKey("HintsOn"))
        {
            if (PlayerPrefs.GetInt("HintsOn") == 1)
            {
                trailObject.SetActive(true);
                HintsToggle.isOn = true;
            }
            else
            {
                trailObject.SetActive(false);
                HintsToggle.isOn = false;
            }

        }
    }

    public void changeAudio()
    {
        UnityEngine.Debug.Log("Audio volume changed");
        float LevelMusicVolume = AudioSlider.value;
        gameMusic.volume = LevelMusicVolume;
        PlayerPrefs.SetFloat("LevelMusicVolume", LevelMusicVolume);
    }

    public void toggleMovenet()
    {
        if (MovenetToggle.isOn)
        {
            // launch Movenet
            myrunMovenet.StartMovenet();
            PlayerPrefs.SetString("MovenetConnected", "T");
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
            trailObject.SetActive(true);
            UnityEngine.Debug.Log("Hints on");
        }
        else
        {
            // turn off hints
            PlayerPrefs.SetInt("HintsOn", 0);
            trailObject.SetActive(false);
            UnityEngine.Debug.Log("Hints off");
        }
    }
}
