using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    //REFERENCE
    public Transform playerCamera;          //kamera
    public CharacterController controller;  //ovladač hráče
    public Transform groundCheck;           //objekt na spodku hráče který sleduje jestli se dotýká země
    public Animator playerAnimator;         //animátor
    public Animator enemyAnimator;
    public Transform enemy;
    public NavMeshAgent enemyAgent;

    //VEŘEJNÉ PROMĚNNÉ
    public float mouseSensitivity = 90f;    //citlivost otáčení
    public float movementSpeed = 10f;       //rychlost pohybu hráče
    public float gravity = -10f;            //velikost gravitace
    public float groundDistance = 0.4f;     //vzdálenost hráče od země
    public LayerMask groundMask;            //"typ" objektů "země"
    public float jumpHeight = 5f;           //výška skoku
    public float detectionAngle = 45f;
    public float detectionDistance = 10;
   

    //OSTATNÍ PROMĚNNÉ
    Vector3 velocity;                       //"energie" hráče
    float xRotation = 0f;                   //pomocná proměnná pomocí které se nastavuje kam až jde otočit kamera
    bool isGrounded;                        //jestli hráč stojí na zemi nebo ne
    bool isAngry;
    
    
    



    //START 
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;                                   //schová kurzor
    }

    //UPDATE
    void Update()
    {
        Gravity();
        CameraRotation();
        PlayerMovement();
        Animations();
        EnemyDetection();
        AgentMovement();
    }

    


    //OTÁČÍ KAMERU ZA MYŠÍ
    void CameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;//ukládá do proměnných "pohyb" myši
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;    

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);                              // zamezuje otočení kamery moc nahoru nebo dolu

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);           //otáčí kameru okolo osy "x"
        transform.Rotate(Vector3.up * mouseX);                                      //otáčí hráče i s kamerou okolo osy "y"
    }

    //POHYB HRÁČE
    void PlayerMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");                         //ukládá do proměnných vstup "WSAD"
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = transform.right * moveHorizontal + transform.forward * moveVertical; //ze vstupů "WSAD" udělá výsledný vektor

        controller.Move(movement * movementSpeed * Time.deltaTime);                 //na základě výsledného vektoru "movement" hýbe s hráčem

        if(Input.GetButtonDown("Jump") && isGrounded == true)                       //pokud je zmáčknutá klávesa pro skok a hráč stojí na zemi
        {
            velocity.y = jumpHeight;                                                //hráči se přidá "energie" směrem nahoru
        }
    }

    //GRAVITACE
    void Gravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); //"isGrounded je pravda pokud hráč stojí na zemi
        
        if (isGrounded == true && velocity.y < 0)                                   //pokud je hráč na zemi a "energie y" hráče je menší než 0
        {
            velocity.y = -2f;                                                       //"energie y" = -2
        }

        velocity.y += gravity * Time.deltaTime;                                     //k "energii y" hráče se přičte gravitace
        controller.Move(velocity * Time.deltaTime);                                 //"energie" pohybuje hráče
    }                                                                       //"Time.deltaTime násobím dvakrát protože gravitace je na druhou

    //ANIMACE
    void Animations()
    {
        if (Input.GetMouseButtonDown(0))                                            //zmáčknutí levého tlačítka na myši => animace levý úder
        {
            playerAnimator.SetTrigger("leftPunch");
        }

        if (Input.GetMouseButtonDown(1))                                            //zmáčknutí pravého tlačítka na myši => animace pravý úder
        {
            playerAnimator.SetTrigger("rightPunch");
        }

        if (Input.GetKeyDown(KeyCode.T))                                            //zmáčknutí "T" => animace "pardonMe"
        {
            playerAnimator.SetTrigger("pardonMe");
        }

    }

    void EnemyDetection()
    {
        Vector3 directionToTarget = transform.position - enemy.position;
        float angle = Vector3.Angle(-transform.forward, directionToTarget);
        float distance = directionToTarget.magnitude;

        if (Mathf.Abs(angle) < detectionAngle && distance < detectionDistance && Input.GetKeyDown(KeyCode.T)) 
        {
            enemyAnimator.SetTrigger("isAngry");
            isAngry = true;
        }
    }

    void AgentMovement()
    {
        if (isAngry)
        {
            enemyAgent.SetDestination(transform.position);
        }
    }
}
