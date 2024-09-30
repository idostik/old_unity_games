using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{

    public float rotatingSpeed;
    
    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 0, rotatingSpeed) * Time.deltaTime);
    }
}
