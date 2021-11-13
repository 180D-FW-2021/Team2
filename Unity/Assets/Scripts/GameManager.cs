using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;

    public static event Action<GameState> OnGameStateChanged;

    void Awake()
    {
        Instance = this;
    }

    void Start() {
        UpdateGameState(GameState.SelectLevel);
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

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch(newState) {
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

        OnGameStateChanged?.Invoke(newState);
    }


}

public enum GameState {
    SelectLevel,
    Playing,
    Paused,
    Resumed,
    Victory
}