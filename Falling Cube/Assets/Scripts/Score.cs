using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public Transform player;
    public Text scoreText;
    private float myScore;


    void Update ()
    {
        myScore = -player.position.y + 6;       //přiřazuje ke skóre hráčovu pozici, převrácená je aby nebyla záporná, +6 aby skóre začínalo na 0 (hráč začíná na y = 6)
        scoreText.text = myScore.ToString("0") + "m";
	}
}
