 using UnityEngine;

[System.Serializable]
public class PlayerScore
{
    public string username;
    public string level;
    public float score;

    public string Stringify() 
    {
        return JsonUtility.ToJson(this);
    }
}