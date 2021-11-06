using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Public: accessible from other scripts
    // Static: don't want to reference this specific PauseMenu script
    // Just want to check from other scripts whether game is paused
    public static bool IsGamePaused = false;
    // in other scripts, can reference this as if (PauseMenu.IsGamePaused)


    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (IsGamePaused)
            {
                Resume();
            } else {
                Pause();
            }
        }
    }
    
    // Public functions to trigger from buttons
    public void Resume() 
    {
        pauseMenuUI.SetActive(false); // Set UI to not active
        Time.timeScale = 1f;  // Set time to normal rate
        IsGamePaused = false;
    }

    void Pause() 
    {
        pauseMenuUI.SetActive(true); // Set game object to active
        Time.timeScale = 0f;  // completely stop time
        IsGamePaused = true;
    }

    public void LoadMenu() 
    {
        Debug.Log("Loading Menu");
        Time.timeScale = 1f;
        // SceneManager.LoadScene("Menu"); // TODO create variable for Menu name rather than hardcoding it
    }

    public void QuitGame() 
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }

}
