using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Canvas gameOverUI;
    public Canvas inGameUI;

    private GameObject spawner;
    private GameObject player;

    

    private void Start()
    {
        gameOverUI.enabled = false;
        inGameUI.enabled = true;

        player = GameObject.FindGameObjectWithTag("Player");
        spawner = GameObject.FindGameObjectWithTag("Spawner");
    }

    public void GameOver()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        gameOverUI.enabled = true;
        inGameUI.enabled = false;

        player.GetComponent<PlayerCharacter>().enabled = false;
        spawner.GetComponent<Spawner>().enabled = false;
        
    }
    
}
 