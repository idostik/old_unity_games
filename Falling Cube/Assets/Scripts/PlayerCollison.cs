using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollison : MonoBehaviour {

    public PlayerMovement movement;
    public Score myScore;
    private bool isAllowedToTrigger = true;     

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Obstacle")
        {
            if (isAllowedToTrigger == true)     // při kolizi s překážkou vypne pohyb a skóre a spustí konečnou obrazovku 
            {
                movement.enabled = false;
                myScore.enabled = false;
                FindObjectOfType<GameManager>().GameOver();
            }
            
        }

        if (collisionInfo.collider.tag =="Finish")
        {
            FindObjectOfType<GameManager>().YouWin();
        }
    }

    private void Update()
    {
        if (Input.GetKey("p"))
        {
            isAllowedToTrigger = false;     //držením klávesy P se při kolizí s překážkou nic nestane
        }
        else
        {
            isAllowedToTrigger = true;
        }
    }

}

