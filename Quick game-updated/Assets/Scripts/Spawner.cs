using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public float spawnRate;
    public float minSpawnRate = 0.25f;
    private float timeBtwSpawn;
    private float decreaseTime;
    public float decreaseTimePercent; //o kolik procent se bude "spawnRate" zmenšovat 

    public GameObject[] enemies;
    public Vector3[] spawnPoints;

    [HideInInspector]
    public Text scoreText;
    static public int myScore = 0;

    private void Start()
    {
        scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
    }

    void Update()
    {
        decreaseTime = (spawnRate / 100) * decreaseTimePercent; // velikost "decreaseTime" je vždy dané procento ze "spawnRate"

        if (timeBtwSpawn <= 0)
        {
            int randType = Random.Range(0, enemies.Length);
            int randPos = Random.Range(0, spawnPoints.Length);

            Instantiate(enemies[randType], spawnPoints[randPos], Quaternion.identity);

            myScore += 1;
            scoreText.text = myScore.ToString();

            timeBtwSpawn = spawnRate;
            Debug.Log(spawnRate);
            if (spawnRate > minSpawnRate)
            {
                spawnRate -= decreaseTime;
            }
            
        }
        else
        {
            timeBtwSpawn -= Time.deltaTime;
        }
    }
}
