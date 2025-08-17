using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterMovement : MonoBehaviour
{
    [Tooltip("A float value determining horizontal movement")]
    float hori;
    [Tooltip("A float value determining vertical movement")]
    float verti;
    [Tooltip("A float value determining the speed of movement")]
    public float speed;
    public float sprint;
    public float walking;


    public float stamina;
    public float maxStamina;
    public bool exausted;
    public float staminaRegen = 0.001f;
    public float staminaExaust = 0.003f;

    public GameManager gameManager;
    public GameObject player;
    private gunCrateType nearbyCrate;
    private Rigidbody2D rb;
    private void Start()
    {
        stamina = maxStamina;
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameObject.Find("GameController").GetComponent<GameManager>();
        player = GameObject.FindWithTag("playerCharacter");
    }

    private void FixedUpdate()
    {
        hori = Input.GetAxisRaw("Horizontal");
        verti = Input.GetAxisRaw("Vertical");
        if(gameManager.inDrill)
        {
            rb.velocity = new Vector2(hori * speed, verti * speed);

        }
        if (Input.GetKey("left shift") && stamina > 0)
        {
            if (!exausted)
            {
                speed = sprint;
                stamina -= staminaExaust;
            }

        }
        else
        {
            speed = walking;

        }
        if (stamina < maxStamina)
        {
            stamina += staminaRegen;
        }
        if (stamina <= 0)
        {
            exausted = true;

        }
        if (stamina >= maxStamina)
        {
            exausted = false;
        }
    }
    void Update()
    {
        //recieves wasd as input assiging a value from -1 to 1



        ////uses change in time, a preset speed variable, and input to move character
        //transform.Translate(Time.deltaTime * Vector2.up * speed * verti);
        //transform.Translate(Time.deltaTime * Vector2.right * speed * hori);

        if (nearbyCrate != null && Input.GetMouseButtonDown(1)) // right click
        {
            var stats = player.GetComponent<playerStats>();
            if (stats.gold >= nearbyCrate.cost)
            {
                Debug.Log("Bought gun type " + nearbyCrate.gunType);
                stats.gold -= nearbyCrate.cost;
                stats.changeGun(nearbyCrate.gunType);
            }
            else
            {
                Debug.Log("Not enough gold!");
            }
        }



    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("asscakses");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("terminal"))
        {
            gameManager.nearTerminal = true;
        }

        if (collision.gameObject.CompareTag("gunCrate"))
        {
            nearbyCrate = collision.gameObject.GetComponent<gunCrateType>();
            Debug.Log("Entered crate zone");
        }
    }
   

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("terminal"))
        {
            gameManager.nearTerminal = false;
        }
        if (collision.gameObject.CompareTag("gunCrate"))
        {
            nearbyCrate = null;
            
        }
    }
}

