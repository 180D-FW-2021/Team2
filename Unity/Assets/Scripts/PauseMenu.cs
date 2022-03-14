using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;

public class PauseMenu : MonoBehaviour
{
    // Public: accessible from other scripts
    // Static: don't want to reference this specific PauseMenu script
    // Just want to check from other scripts whether game is paused
    // public static bool IsGamePaused = false;
    // in other scripts, can reference this as if (PauseMenu.IsGamePaused)


    public GameObject pauseMenuUI;

    void Awake() {
        GameManager.OnGameStateChanged += GameManagerOnOnGameStateChanged;
    }

    void OnDestroy() {
        GameManager.OnGameStateChanged -= GameManagerOnOnGameStateChanged;
    }

    private void GameManagerOnOnGameStateChanged(GameState state) {
        if (state == GameState.Paused) {
            Pause();
        } else if (state == GameState.Resumed) {
            Resume();
        }
        
    }

    // // Update is called once per frame
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Escape)) 
    //     {
    //         if (IsGamePaused)
    //         {
    //             Resume();
    //         } else {
    //             Pause();
    //         }
    //     }
    // }
    
    // Public functions to trigger from buttons
    public void Resume() 
    {
        pauseMenuUI.SetActive(false); // Set UI to not active
        Time.timeScale = 1f;  // Set time to normal rate
        GameManager.Instance.UpdateGameState(GameState.Playing);
        // IsGamePaused = false;
    }

    public void Pause() 
    {
        pauseMenuUI.SetActive(true); // Set game object to active
        Time.timeScale = 0f;  // completely stop time
        // IsGamePaused = true;
    }

    public void LoadMenu() 
    {
        UnityEngine.Debug.Log("Loading Menu");
        Time.timeScale = 1f;
        // SceneManager.LoadScene("Menu"); // TODO create variable for Menu name rather than hardcoding it
    }

    public void QuitGame() 
    {
        foreach (Process p in Process.GetProcessesByName("position_tracking"))
        {
            p.CloseMainWindow();
        }
        UnityEngine.Debug.Log("Quitting Game");
        GameManager.Instance.UpdateGameState(GameState.Quitting);
        // Application.Quit();
    }

}
