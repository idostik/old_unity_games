using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UI;
    
public class Enemy : MonoBehaviour
{
    public GameObject collisionSound;
    public GameObject particleEffect;

    public float speed;
    public float maxDistance = -9;
    //public Text scoreText;
    //private int myScore = 0;

    private void Start()
    {
        //scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
        
    }

    void FixedUpdate()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        if(transform.position.x <= maxDistance)
        {
            FindObjectOfType<GameManager>().GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Cannon"))
        {
            FindObjectOfType<GameManager>().GameOver();
        }

        if (other.tag == gameObject.tag)
        {
            //myScore += 1;
            //Debug.Log(myScore);
            //scoreText.text = myScore.ToString();
            
            Instantiate(collisionSound, transform.position, Quaternion.identity);
            Instantiate(particleEffect, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else
        {
            FindObjectOfType<GameManager>().GameOver();
        }
    }
}
