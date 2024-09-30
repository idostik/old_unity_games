using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public GameObject[] projectiles;

    private Vector2 targetPos;
    public float Yincrement = 2.5f;
    public float speed = 5f;
    public float maxHeight;
    public float minHeight;

    public AudioSource audioSrc;
    public GameObject particleEffect;

    private void Start()
    {
        targetPos = transform.position;
    }

    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Q))
        {
            Instantiate(projectiles[0], transform.position, Quaternion.identity);
            Instantiate(particleEffect, transform.position, Quaternion.identity);
        } else if (Input.GetKeyDown(KeyCode.W))
        {
            Instantiate(projectiles[1], transform.position, Quaternion.identity);
            Instantiate(particleEffect, transform.position, Quaternion.identity);
        } else if (Input.GetKeyDown(KeyCode.E))
        {
            Instantiate(projectiles[2], transform.position, Quaternion.identity);
            Instantiate(particleEffect, transform.position, Quaternion.identity);
        } else if (Input.GetKeyDown(KeyCode.R))
        {
            Instantiate(projectiles[3], transform.position, Quaternion.identity);
            Instantiate(particleEffect, transform.position, Quaternion.identity);
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

       if(Input.GetKeyDown(KeyCode.UpArrow) && transform.position.y < maxHeight)
        {
            targetPos = new Vector2(transform.position.x, transform.position.y + Yincrement);
            audioSrc.Play();
        }
       else if (Input.GetKeyDown(KeyCode.DownArrow) && transform.position.y > minHeight)
        {
            targetPos = new Vector2(transform.position.x, transform.position.y - Yincrement);
            audioSrc.Play();
         
        }
    }
}
