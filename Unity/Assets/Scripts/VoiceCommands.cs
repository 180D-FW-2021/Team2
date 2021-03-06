using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

// reference: https://www.youtube.com/watch?v=29vyEOgsW8s

public class VoiceCommands : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

	public GameManager GameManagerScript;

    void Start() {
        Debug.Log("We are starting :)");

		GameManagerScript = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
        
    	keywords.Add("start", StartCallback);
		keywords.Add("resume", StartCallback);
		keywords.Add("pause", StopCallback);
    	keywords.Add("stop", StopCallback);
		keywords.Add("menu", MenuCallback);
    	keywords.Add("exit", ExitCallback);
		keywords.Add("quit", ExitCallback);

		//string[] array1 = {"start", "stop", "exit", "one", "two", "three", "quit"};
    	
		// Create the keyword recognizer and tell it what to recognize
		keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
    	
		// keywordRecognizer = new KeywordRecognizer(array1);
		
		// Register OnPhraseRecognized event
		keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
    	keywordRecognizer.Start();
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech) {
    	// Debug.Log(speech.text);
    	keywords[speech.text].Invoke();
    }

    private void StartCallback() {
    	Debug.Log("Resuming game");
		GameManagerScript.UpdateGameState(GameState.Resumed);
		// PauseMenu.Resume();
    }

    private void StopCallback() {
    	Debug.Log("Pausing game");
		GameManagerScript.UpdateGameState(GameState.Paused);
		// PauseMenu.Pause();
    }

	private void MenuCallback() {
    	Debug.Log("Loading Main menu");
		GameManagerScript.UpdateGameState(GameState.MainMenu);
		// PauseMenu.Pause();
    }

    private void ExitCallback() {
    	Debug.Log("Exitting game");
		GameManagerScript.UpdateGameState(GameState.Quitting);
		// PauseMenu.QuitGame();
    }
}
