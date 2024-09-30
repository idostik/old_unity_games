using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highscore : MonoBehaviour
{
    public Text highScore;
    private int currentScore;

    void Start()
    {
        currentScore = Spawner.myScore;
        highScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();


        if (currentScore > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
            highScore.text = currentScore.ToString();
        }
    }

   
}
