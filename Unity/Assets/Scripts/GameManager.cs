using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* GameManager object created by adapting the tutorial provided here:
    https://www.youtube.com/watch?v=4I0vonyqMi8


    Singleton guide (ensure there is only one singleton)
    https://www.youtube.com/watch?v=5p2JlI7PV1w

    Set GameObjects by finding them by name (rather than dragging in)
    https://docs.unity3d.com/ScriptReference/GameObject.Find.html
*/

public class GameManager : MonoBehaviour
{   
    // Use this Instance variable to reference the GameManager object in other scripts
    // E.g. can call GameManager.Instance.UpdateGameState(state variable) to update states
    public static GameManager Instance;

    // Variable keeps track of GameState
    public GameState State;

    // Use this event to have other scripts be alerted when GameState changes
    public static event Action<GameState> OnGameStateChanged;

    private int selectedLevel;

    // public GameObject MenuCanvas;
    // public GameObject StartMenuUI;
    // public GameObject MainMenuUI;
    // public GameObject LevelSelectorUI;

    // Initialize variable before program starts
    void Awake()
    {
        if (Instance == null) {
            // MenuCanvas = GameObject.Find("/Canvas");
            // if (MenuCanvas == null) {
            //     Debug.Log("ERROR");
            // }

            // StartMenuUI = GameObject.Find("Canvas/StartMenu");
            // MainMenuUI = GameObject.Find("Canvas/MainMenu");
            // LevelSelectorUI = GameObject.Find("Canvas/LevelSelector");
            // if (MainMenuUI == null) {
            //     Debug.Log("ERROR");
            // }
            Debug.Log("here");
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Debug.Log("here2");
            Destroy(gameObject);
        }

    }

    // Set state on game start
    void Start() {
        UpdateGameState(GameState.StartScreen);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (State == GameState.Paused){
                UpdateGameState(GameState.Resumed);
            } else {
                UpdateGameState(GameState.Paused);
            }
        }
    }

    // Function that is invoked whenever game state is changed
    public void UpdateGameState(GameState newState, int level = 0)
    {
        State = newState;

        switch(newState) {
            case GameState.StartScreen:
                SceneManager.LoadScene("StartScreen");
                break;
            case GameState.MainMenu:
                SceneManager.LoadScene("MainMenu");
                break;
            case GameState.HelpMenu:
                SceneManager.LoadScene("HelpMenu");
                break;
            case GameState.SelectLevel:
                SceneManager.LoadScene("LevelSelector");
                break;
            case GameState.ConfirmLevelSelection:
                selectedLevel = level;
                SceneManager.LoadScene("AreYouReadyScreen");
                break;
            case GameState.WaitForMovenet:
                break;
            case GameState.LoadSelectedLevel:
                SceneManager.LoadScene("MazeLevel_" + selectedLevel.ToString());
                Time.timeScale = 1f; // ensure time is moving
                State = GameState.Playing; 
                break;
            case GameState.Playing:
                break;
            case GameState.Paused:
                break;
            case GameState.Resumed:
                break;
            case GameState.Victory:
                HandleVictory();
                break;   
            case GameState.Quitting:
                PlayerPrefs.DeleteKey("MovenetConnected");
                Application.Quit();
                break;          
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        // If other scripts are suscribed to changes in GameState, inform them
        OnGameStateChanged?.Invoke(newState);
    }

    private void HandleVictory()
    {
        Debug.Log("Change to end screen");
        SceneManager.LoadScene("EndScreen");
    }

}

// enumeration of all possible game states
public enum GameState {
    StartScreen,
    MainMenu,
    SelectLevel,
    HelpMenu,
    ConfirmLevelSelection,
    WaitForMovenet,
    LoadSelectedLevel,
    Playing,
    Paused,
    Resumed,
    Victory,
    Quitting
}