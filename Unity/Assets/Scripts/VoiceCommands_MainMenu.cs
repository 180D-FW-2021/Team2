using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

// reference: https://www.youtube.com/watch?v=29vyEOgsW8s

public class VoiceCommands_MainMenu : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

	public GameManager GameManagerScript;

    void Start() {
		GameManagerScript = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
        
		keywords.Add("level", LevelsCallback);
    	keywords.Add("levels", LevelsCallback);
		keywords.Add("help", HelpCallback);
        keywords.Add("settings", SettingsCallback);
        keywords.Add("quit", QuitCallback);
    	keywords.Add("back", BackCallback);
		keywords.Add("log", BackCallback); // TODO: revisit this

		// Create the keyword recognizer and tell it what to recognize
		keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
    	
		// Register OnPhraseRecognized event
		keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
    	keywordRecognizer.Start();
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech) {
    	keywords[speech.text].Invoke();
    }

    private void LevelsCallback() {
    	Debug.Log("to level selection");
		// MainMenuScripts.To_LevelSelector();
		GameManagerScript.UpdateGameState(GameState.SelectLevel);
    }

	private void HelpCallback() {
		GameManagerScript.UpdateGameState(GameState.HelpMenu);
    	Debug.Log("Help Menu");
    }

    private void SettingsCallback()
    {
        GameManagerScript.UpdateGameState(GameState.SettingsMenu);
        Debug.Log("Settings Menu");
    }

    private void QuitCallback() {
    	Debug.Log("Quitting");
		GameManagerScript.UpdateGameState(GameState.Quitting);
    }

    private void BackCallback() {
    	Debug.Log("Back to Start Screen");
		PlayerPrefs.SetString("Username", "");
        Debug.Log(PlayerPrefs.GetString("Username"));
		GameManagerScript.UpdateGameState(GameState.StartScreen);
    }
}
