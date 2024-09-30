﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public bool hasCollided = false;                               //proměnná na kontrolu kolizí
    public float timeBtwHits = 1.5f;                                //čas po úspěšném úderu, kdy se nepočítají kolize, aby se 1 úder nepočítal 2x
    private float elapsedTime;
    [HideInInspector]
    public bool playerGotHit = false;                                //pomocí této proměnné se ve scriptu "PlayerController" počítá poškození


    private void Start()
    {
        elapsedTime = timeBtwHits;
    }

    public void OnTriggerEnter(Collider other)                      //pokud došlo ke kolizi s pěstí a v předchozí 1.5s (timeBtwHits) k němu nedošlo
    {
        if (other.gameObject.tag == "Enemy Fist" && hasCollided == false)
        {
            hasCollided = true;
            playerGotHit = true;
            Debug.Log("Hit");
        }
    }

    private void Update()                                           //po dobu timeBtwHits se nepočítají kolize                           
    {
        if (hasCollided == true)
        {
            elapsedTime -= Time.deltaTime;
            playerGotHit = false;

            if (elapsedTime <= 0)
            {
                hasCollided = false;
                elapsedTime = timeBtwHits;
            }
        }
    }
}
