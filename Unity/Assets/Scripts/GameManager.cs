using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* GameManager object created by adapting the tutorial provided here:
    https://www.youtube.com/watch?v=4I0vonyqMi8

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

    // Initialize variable before program starts
    void Awake()
    {
        Instance = this;
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
    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch(newState) {
            case GameState.StartScreen:
                break;
            case GameState.MainMenu:
                break;
            case GameState.SelectLevel:
                // HandleSelectLevel();
                break;
            case GameState.Playing:
                break;
            case GameState.Paused:
                break;
            case GameState.Resumed:
                break;
            case GameState.Victory:
                break;             
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        // If other scripts are suscribed to changes in GameState, inform them
        OnGameStateChanged?.Invoke(newState);
    }


}

// enumeration of all possible game states
public enum GameState {
    StartScreen,
    MainMenu,
    SelectLevel,
    Playing,
    Paused,
    Resumed,
    Victory
}