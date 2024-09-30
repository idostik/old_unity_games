using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public Rigidbody rb;
    public float speed; //rychlost pohybu
    public float downwardForce;
    public float pushForce;

    void Push()
    {
        rb.AddForce(0, 0, pushForce);
    }

    void Start()
    {
        Invoke("Push", 0.5f);   //postrčení krychle z podstavce na začátku hry
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        rb.AddForce(0, -downwardForce * Time.deltaTime, 0); //rychlost letu směrem dolů

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        rb.AddForce(movement * speed * Time.deltaTime, ForceMode.VelocityChange);
    }
        
}
