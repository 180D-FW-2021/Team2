using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

// reference: https://www.youtube.com/watch?v=29vyEOgsW8s

public class VoiceCommandsTesting : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;

    void Start() {
 
	string[] array1 = {"start", "enter", "stop", "exit", "quit", "levels", "menu", "settings", "zero", "one", "two", "three", "help", "yes", "ok", "no", "back", "won"};

		// "won", 
    	
		// Create the keyword recognizer and tell it what to recognize
		//keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
    	
		 keywordRecognizer = new KeywordRecognizer(array1);
		
		// Register OnPhraseRecognized event
		keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
    	keywordRecognizer.Start();
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech) {
    	Debug.Log(speech.text);
    	//keywords[speech.text].Invoke();
    }


}
