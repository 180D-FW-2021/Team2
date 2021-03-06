using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SubmitScoreScript : MonoBehaviour
{
    string level;
    float score;
    string username;

    void Start()
    {
        // Check whether playing as guest or signed in
        string playingAsGuest = PlayerPrefs.GetString("PlayingAsGuest");

        if (playingAsGuest == "F") {
            // Signed in, so get player username/level/score
            username = PlayerPrefs.GetString("Username");
            level = PlayerPrefs.GetString("PrevScene");
            score = PlayerPrefs.GetFloat("finalTime");

            SubmitScore();
        }
    }

    // ref: https://www.mongodb.com/developer/how-to/sending-requesting-data-mongodb-unity-game/
    public void SubmitScore() 
    {
        Debug.Log("submitting score for " + username);
        string url = "https://amaze-webapp.herokuapp.com/api/insert";
        PlayerScore scoreObj = new PlayerScore();
        scoreObj.username = username;
        scoreObj.level = level;
        scoreObj.score = score;
        string json = scoreObj.Stringify();
        Debug.Log(json);
        StartCoroutine(UploadScore(url, json));
    }

    IEnumerator UploadScore(string url, string json)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("score submitted!");
            }
        }
    }

}
