using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingMovenet : MonoBehaviour
{
    // Public: accessible from other scripts
    // Static: don't want to reference this specific PauseMenu script
    // Just want to check from other scripts whether game is paused
    // public static bool IsGamePaused = false;
    // in other scripts, can reference this as if (PauseMenu.IsGamePaused)
    public GameObject LoadingMovenetUI;

    public GameManager GameManagerScript;

    private string connectedToMqtt;
    private string playingAsGuest;

    void Awake() {
        GameManager.OnGameStateChanged += GameManagerOnOnGameStateChanged;
        GameManagerScript = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;

        playingAsGuest = PlayerPrefs.GetString("PlayingAsGuest");
    }

    void OnDestroy() {
        GameManager.OnGameStateChanged -= GameManagerOnOnGameStateChanged;
    }

    void Update() {
        connectedToMqtt = PlayerPrefs.GetString("MovenetConnected", "F");
        if (connectedToMqtt == "T" && LoadingMovenetUI.activeSelf) {
            Debug.Log("I am ready to load scene");
            GameManagerScript.UpdateGameState(GameState.LoadSelectedLevel);
        }
    }

    private void GameManagerOnOnGameStateChanged(GameState state) {
        if (state == GameState.WaitForMovenet) {
            DisplayPopUp();
        }
        // } else if (state == GameState.Resumed) {
        //     Resume();
        // }
        
    }

    // Public functions to trigger from buttons
    // public void Resume() 
    // {
    //     pauseMenuUI.SetActive(false); // Set UI to not active
    //     Time.timeScale = 1f;  // Set time to normal rate
    //     GameManager.Instance.UpdateGameState(GameState.Playing);
    // }

    public void DisplayPopUp() 
    {
        if (playingAsGuest == "T") {
            Debug.Log("Load Scene because not using Movenet");
            GameManagerScript.UpdateGameState(GameState.LoadSelectedLevel);
        } else {
            LoadingMovenetUI.SetActive(true); // Set game object to active
        }
    }

}
