using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform player;
    public Vector3 offset;

    private void Update()
    {
        transform.position = player.position + offset;      //kamera si bude vždy držet vzdálenost "offset" za krychlí
    }
}
