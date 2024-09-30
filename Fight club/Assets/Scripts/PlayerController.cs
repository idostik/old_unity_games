using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    //REFERENCE ----------------------------(V Unity editoru jim jsou přiděleny různé komponenty)
    public CharacterController controller;  
    public Transform groundCheck;           //objekt pod hráčem, který sleduje, jestli se dotýká země
    public Transform lookTarget;            //bod na protivníkovi, na který směřuje kamera
    public Animator playerAnimator;         //pouští hráčovi animace
    public Animator enemyAnimator;          //pouští protivníkovi animace
    public Transform enemy;                 //poloha a rotace protivníka
    public Transform enemyLookTarget;       //bod na hráči, ke kterému bude protivník pořád čelem
    public Rigidbody faceRb;                //"Rigidbody" na obličeji protivníka (komponent co řeší "fyzikální síly")
    public Transform cam;                   //poloha a rotace kamery
    //public NavMeshAgent enemyAgent;

    //VEŘEJNÉ PROMĚNNÉ
    public float movementSpeed = 10f;
    public float gravity = -10f;            //velikost gravitace
    public float groundDistance = 0.4f;     //vzdálenost, ve které se kontroluje, jestli hráč stojí na zemi (viz Gravitace())
    public LayerMask groundMask;            //"typ" objektů "země"
    public float jumpHeight = 5f;
    public float maxDist;                   //max vzdálenost mezi boxery. Když se překročí, protivník se začne přibližovat
    //public float minDist;
    public float waitTime = 2f;             //po jak dlouhé době od překročení max vzdálenosti se protivník začne přibližovat
    //public float waitTimeBck = 0.4f;
    public float atkInvokeDelay = 5f;       //po jak dlouhé době od spuštění hry začne protivník útočit
    public float minAtkRate = 2f;           //minimální pauza mezi útoky protivníka
    public float maxAtkRate = 8f;           //maximální pauza mezi útoky protivníka
    public float knockback = 1f;            //síla odhození protivníka při K.O.
    public float enemyDodgeAccuracy = 80f;  // kolik procent útoků se protivník vyhne
    public int lightPunchDamage = 1;        //poškení úderů
    public int heavyPunchDamage = 3;
    public int minKODmg = 15;               //minimální poškození potřebné k vítězství (pak se zvyšuje šance na K.O.)
    //public float camShakeDuration = 0.2f;
    //public float camShakeMagnitude = 0.4f;
    //public float mouseSensitivity = 90f;
    //public float detectionAngle = 45f;
    //public float detectionDistance = 10;

    //SOUKROMÉ PROMĚNNÉ
    private Vector3 velocity;                       //"energie" hráče
    private bool isGrounded;                        //jestli hráč stojí na zemi nebo ne
    private float elapsedTime;
    private bool enemyMoveFwd = false;              //určuje kdy se má protivník začít přibližovat
    private string[] attacks = new string[] { "LLP", "LHP", "RLP", "RHP" };     //kolekce útoků (LLP = Left Light Attack atd.)
    private bool enemyGotHit;                       //používám pro načtení hodnoty ze scriptu "Collision"
    private bool playerGotHit;
    private bool hasCollided;
    private bool isAlive = true;
    private int enemyDmg = 0;                       //protivníkovo utržené poškození
    private int playerDmg = 0;                      //hráčovo utržené poškození
    private bool enemyCanDie = false;
    private bool playerCanDie = false;

    //bool enemyMoveBck = false;
    //bool isAngry;
    
    //-----------------------------------------------------------------------------------------------------------------------------------------
    
    //START 
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;                                   //schová kurzor

        InvokeRepeating("EnemyAtk", atkInvokeDelay, Random.Range(minAtkRate, maxAtkRate));  //v náhodných intervalech volá funkci EnemyAtk()

    }

    //UPDATE
    void Update()
    {
        if (isAlive)                                                                //aby se protivník nehýbal když je K.O.
        {
            CameraRotation();
            EnemyMovement();
            PlayerMovement();
        }

        Gravity();
        Animations();
        EnemyDmgControll();
        PlayerDmgControll();
        //EnemyDetection();
        //AgentMovement();
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------


    //ROTACE KAMERY A PROTIVNÍKA
    void CameraRotation()
    {
        transform.LookAt(lookTarget);                                               // otáčí boxery čelem proti sobě
        enemy.LookAt(enemyLookTarget);
    }

    //HRÁČ - POHYB
    void PlayerMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");                         //ukládá do proměnných vstup "WSAD"
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = transform.right * moveHorizontal + transform.forward * moveVertical; //ze vstupů "WSAD" udělá výsledný vektor

        controller.Move(movement * movementSpeed * Time.deltaTime);                 //na základě výsledného vektoru "movement" hýbe s hráčem

        if(Input.GetButtonDown("Jump") && isGrounded == true)                       //pokud je zmáčknutá klávesa pro skok a hráč stojí na zemi
        {
            velocity.y = jumpHeight;                                                //hráči se přidá "energie" směrem nahoru (hráč skočí)
        }
    }

    //PROTIVNÍK - POHYB
    void EnemyMovement()
    {
        float distance = Vector3.Distance(transform.position, enemy.position);      //vzdálenost mezi boxery

        //Pohyb dopředu
        if (distance > maxDist)                                                     //pokud se překročí max vzdálenost, začne se odečítat čas waitTime
        {                                                                           //když se čas odečte, začne se nepřítel přibližovat
            if (elapsedTime <= 0)
            {
                enemyMoveFwd = true;
            }
            else
            {
                elapsedTime -= Time.deltaTime;
            }
        }
        else
        {
            elapsedTime = waitTime;
            enemyMoveFwd = false;
        }
        
        if(enemyMoveFwd == true)                                                    //nepřítel se přiblíží k hráči
        {
            enemy.position = Vector3.MoveTowards(enemy.position, enemyLookTarget.position, movementSpeed * Time.deltaTime);
        }

        //Pohyb dozadu
        //if (distance < minDist)
        //{
        //    if (elapsedTime1 <= 0)
        //    {
        //        enemyMoveBck = true;
        //    }
        //    else
        //    {
        //        elapsedTime1 -= Time.deltaTime;
        //    }
        //}
        //else
        //{
        //    elapsedTime1 = waitTimeBck;
        //    enemyMoveBck = false;
        //}

        //if (enemyMoveBck == true)
        //{
        //    enemy.position = Vector3.MoveTowards(enemy.position, enemyLookTarget.position, -movementSpeed * Time.deltaTime);
        //}
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
    }

    //ANIMACE
    void Animations()
    {
        
        //HRÁČ
        //Úhyby                                                                     zmáčknutí daných kláves spustí náležitý úhyb
        if (Input.GetKeyDown(KeyCode.E) && !Input.GetKey(KeyCode.LeftShift))
        {
            playerAnimator.SetTrigger("RLDodge");
        }

        if (Input.GetKeyDown(KeyCode.Q) && !Input.GetKey(KeyCode.LeftShift))
        {
            playerAnimator.SetTrigger("LLDodge");
        }

        if (Input.GetKeyDown(KeyCode.Q) && Input.GetKey(KeyCode.LeftShift))
        {
            playerAnimator.SetTrigger("LHDodge");
        }

        if (Input.GetKeyDown(KeyCode.E) && Input.GetKey(KeyCode.LeftShift))
        {
            playerAnimator.SetTrigger("RHDodge");

        }
        //Útoky                                                                     zmáčknutí daných kláves spustí náležitý útok
        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
        {
            playerAnimator.SetTrigger("LLPunch");
        }

        if (Input.GetMouseButtonDown(1) && !Input.GetKey(KeyCode.LeftShift))
        {
            playerAnimator.SetTrigger("RLPunch");
            
        }

        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift))
        {
            playerAnimator.SetTrigger("LHPunch");
        }

        if (Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.LeftShift))
        {
            playerAnimator.SetTrigger("RHPunch");
        }

        //PROTIVNÍK
        //otáčení                                                                   při pohybu spouští animace protivníka
        if (Input.GetKey(KeyCode.A))
        {
            enemyAnimator.SetBool("walkingLeft", true);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            enemyAnimator.SetBool("walkingLeft", false);
        }
        if (Input.GetKey(KeyCode.D))
        {
            enemyAnimator.SetBool("walkingRight", true);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            enemyAnimator.SetBool("walkingRight", false);
        }
        //pohyb 
        if(enemyMoveFwd == true)
        {
            enemyAnimator.SetBool("walkingForward", true);
        }
        else
        {
            enemyAnimator.SetBool("walkingForward", false);
        }
        //úhyby
        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift)) //pokud hráč zaútočí, protivník má procentuální šanci na úhyb
        {
            if (Random.Range(0, 100) <= enemyDodgeAccuracy)
            {
                enemyAnimator.SetTrigger("LLD");
            }
        }

        if (Input.GetMouseButtonDown(1) && !Input.GetKey(KeyCode.LeftShift))
        {
            if (Random.Range(0, 100) <= enemyDodgeAccuracy)
            {
                enemyAnimator.SetTrigger("RLD");
            }

        }

        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift))
        {
            if (Random.Range(0, 100) <= enemyDodgeAccuracy)
            {
                enemyAnimator.SetTrigger("LHD");
            }
        }

        if (Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.LeftShift))
        {
            if (Random.Range(0, 100) <= enemyDodgeAccuracy)
            {
                enemyAnimator.SetTrigger("RHD");
            }
        }
    }

    //PROTIVNÍK - ÚTOK
    void EnemyAtk()
    {
        int randTrig = Random.Range(0, attacks.Length);                             //provede náhodný ze 4 útoků

        enemyAnimator.SetTrigger(attacks[randTrig]);
    }

    //PROTIVNÍK - UTRŽENÉ POŠKOZENÍ
    void EnemyDmgControll()
    {
        enemyGotHit = GameObject.Find("face").GetComponent<Collision>().enemyGotHit;        //bere proměnnou ze scriptu "Collision"

        if(enemyGotHit == true)
        {
            Debug.Log("protivník utržil:" + enemyDmg);
            enemyAnimator.SetTrigger("isHit");

            if (AnimIsPlaying(playerAnimator, "LeftLightPunch") || AnimIsPlaying(playerAnimator, "RightLightPunch"))        //podle animace útoku udělí poškození
            {
                EnemyTakesDamage("lightPunch");
            }

            if (AnimIsPlaying(playerAnimator, "LeftHeavyPunch") || AnimIsPlaying(playerAnimator, "RightHeavyPunch"))
            {
                EnemyTakesDamage("heavyPunch");
            }
        }

        if (enemyDmg >= minKODmg && enemyCanDie == false)                                   //nejdřív je potřeba udělit 15 poškození (minKODmg")
        {                                                                                   //potom se s každým dalším poškozením zvyšuje šance na K.O. (viz "EnemyTakesDamgae()")
            enemyDmg = 1;
            enemyCanDie = true;
        }

    }

    //PROTIVNÍK - UTRŽENÉ POŠKOZENÍ > K.O.
    void EnemyTakesDamage(string punchType)
    {
        if (punchType == "lightPunch")
        {
            enemyDmg += lightPunchDamage;
        }
        if (punchType == "heavyPunch")
        {
            enemyDmg += heavyPunchDamage;
        }

        if (enemyCanDie == true && Random.Range(0, 100) < enemyDmg)                               //procentuální šance na K.O. zvyující se s uděleným poškozením
        {
            enemyAnimator.enabled = false;                                                  //vypne animace a tím umožní "ragdoll"
            isAlive = false;
            Vector3 hitDirection = enemy.position - transform.position;                     //odhodí nepřítele dozadu
            faceRb.AddForce(hitDirection * knockback * Time.deltaTime);
        }
    }

    ////HRÁČ - URTŽENÉ POŠKOZENÍ
    void PlayerDmgControll()
    {
        playerGotHit = GameObject.Find("spine.003").GetComponent<PlayerCollision>().playerGotHit;

        if (playerGotHit == true)
        {
            if (AnimIsPlaying(enemyAnimator, "LeftLightPunch") && !AnimIsPlaying(playerAnimator, "LeftLightDodge"))     //Pokud hráč použije správný úhyb, poškození se nezapoačte (udělat to přes kolize se nepodařilo)
            {                                                                                                           //Jinak stejné jako ve funkci "EnemyDmgControll"
                PlayerTakesDamage("lightPunch");
            }
            else if (AnimIsPlaying(enemyAnimator, "RightLightPunch") && !AnimIsPlaying(playerAnimator, "RightLightDodge"))
            {
                PlayerTakesDamage("lightPunch");
            }
            else if (AnimIsPlaying(enemyAnimator, "LeftHeavyPunch") || AnimIsPlaying(enemyAnimator, "RightHeavyPunch"))
            {
                PlayerTakesDamage("heavyPunch");
            }
            else
            {
                Debug.Log("úhyb");
            }
        }

        if (playerDmg >= minKODmg && playerCanDie == false)
        {
            playerDmg = 1;
            playerCanDie = true;
        }
    }

    //HRÁČ - UTRŽENÉ POŠKOZENÍ > K.O.
    void PlayerTakesDamage(string punchType)
    {
        if (punchType == "lightPunch")
        {
            playerDmg += lightPunchDamage;
            Debug.Log("hráč utržil:" + playerDmg);
        }
        if (punchType == "heavyPunch")
        {
            playerDmg += heavyPunchDamage;
            Debug.Log("hráč utržil:" + playerDmg);
        }

        if (playerCanDie == true && Random.Range(0, 100) < enemyDmg)
        {
            isAlive = false;
            Debug.Log("-------------PROHRÁL JSI-----------");
        }

        playerAnimator.SetTrigger("isHit");

        //StartCoroutine(CameraShake(camShakeDuration, camShakeMagnitude));
    }

    //KONTROLA ANIMACÍ (která teď hraje)
    bool AnimIsPlaying(Animator animator, string animName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animName))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //NEPOUŽÍVANÉ FUNKCE

    //CAMERA SHAKE
    //IEnumerator CameraShake(float duration, float magnitude)
    //{
    //    Vector3 originalPos = cam.localPosition;
    //    float elapsedTime = 0.0f;

    //    while (elapsedTime < duration)
    //    {
    //        float x = Random.Range(-1f, 1f) * magnitude;
    //        float y = Random.Range(-1f, 1f) * magnitude;
    //        float z = Random.Range(-1f, 1f) * magnitude;

    //        cam.localPosition += new Vector3(x, y, z);

    //        elapsedTime += Time.deltaTime;

    //        yield return null;
    //    }

    //    cam.localPosition = originalPos;
    //}

    //void EnemyDetection()
    //{
    //    Vector3 directionToTarget = transform.position - enemy.position;
    //    float angle = Vector3.Angle(-transform.forward, directionToTarget);
    //    float distance = directionToTarget.magnitude;

    //    if (Mathf.Abs(angle) < detectionAngle && distance < detectionDistance && Input.GetKeyDown(KeyCode.T)) 
    //    {
    //        enemyAnimator.SetTrigger("isAngry");
    //        isAngry = true;
    //    }
    //}

    //void AgentMovement()
    //{
    //    if (isAngry)
    //    {
    //        enemyAgent.SetDestination(transform.position);
    //    }
    //}
}
