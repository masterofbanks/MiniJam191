using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NavMeshPlus.Components;

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
    public GameObject navSurf;
    public GameObject tooltip;
    public shooting bullet;
    private gunCrateType nearbyCrate;
    private doorProperties nearbyDoor;
    private portalProperties nearbyPortal;
    private Rigidbody2D rb;
    private void Start()
    {
        stamina = maxStamina;
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameObject.Find("GameController").GetComponent<GameManager>();
        player = GameObject.FindWithTag("playerCharacter");
        navSurf = GameObject.FindWithTag("navigationSurface");
        bullet = GameObject.FindGameObjectWithTag("gun").GetComponent<shooting>();
    
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

        if (nearbyCrate != null && Input.GetMouseButtonDown(1))
        {
            var stats = player.GetComponent<playerStats>();
            if (stats.gold >= nearbyCrate.cost)
            {
                stats.gold -= nearbyCrate.cost;

                // Instantiate the new gun and get its reference directly
                GameObject newGun = stats.changeGun(nearbyCrate.gunType); // make changeGun return the new gun
                if (newGun != null)
                {
                    bullet = newGun.GetComponent<shooting>();
                }
            }
        }
        if (nearbyDoor != null && Input.GetMouseButtonDown(1)) // right click
        {
            var stats = player.GetComponent<playerStats>();
            if (stats.gems >= nearbyDoor.doorPrice)
            {

                stats.gems -= nearbyDoor.doorPrice;
                nearbyDoor.GetComponent<doorProperties>().turnSpawnersOn();
                Destroy(nearbyDoor.gameObject);
                navSurf.GetComponent<NavMeshSurface>().BuildNavMeshAsync();
            }

        }
        if (nearbyPortal != null && Input.GetMouseButtonDown(1))
        {
            var stats = player.GetComponent<playerStats>();
            if (stats.gems >= nearbyPortal.upgradecostGems && stats.gold >= nearbyPortal.upgradecostGold)
            {

                stats.gems -= nearbyPortal.upgradecostGems;
                stats.gold -= nearbyPortal.upgradecostGold;
                if (bullet.piercing)
                    bullet.upgradeBulletStats(bullet.gunVelocity * 0.25f, bullet.gunBulletRange*0.25f , bullet.gunDamage*0.25f, bullet.gunAccuracy * 0.25f, true, bullet.maxDamage*0.25f);
                else if (bullet.gunAccuracy < 0.5)
                {
                    bullet.upgradeBulletStats(bullet.gunVelocity * 0.25f, bullet.gunBulletRange * 0.25f, bullet.gunDamage * 0.25f, 0, true, bullet.maxDamage * 0.25f);
                }
                else
                {
                    bullet.upgradeBulletStats(15, 5, 15, 0.15f, true, 30);
                }

            }

        }
        if (nearbyCrate != null)
        {
            tooltip.GetComponent<toolTip>().Show("Right Click to Interact \n" + nearbyCrate.gunString + " " + nearbyCrate.cost + " Gold");
        }
        else if (nearbyDoor != null)
        {
            tooltip.GetComponent<toolTip>().Show("Right Click to Interact \n" + nearbyDoor.doorPrice + " Gems");
            
        }else if(gameManager.nearTerminal)
        {
            tooltip.GetComponent<toolTip>().Show("Right Click to Open Map");
        }else if(nearbyPortal != null)
        {
            tooltip.GetComponent<toolTip>().Show("Right Click to upgrade \n" + nearbyPortal.upgradecostGems + " Gems\n" + nearbyPortal.upgradecostGold + "Gold");
        }
        else
        {
            tooltip.GetComponent<toolTip>().Hide();
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
        if (collision.gameObject.CompareTag("door"))
        {
            nearbyDoor = collision.gameObject.GetComponent<doorProperties>();
           
        }
        if (collision.gameObject.CompareTag("upgradePortal"))
        {
            nearbyPortal = collision.gameObject.GetComponent<portalProperties>();
        
        }
    }
   

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("terminal"))
        {
            gameManager.nearTerminal = false;
            gameManager.GoBackToMainCam();
        }
        if (collision.gameObject.CompareTag("gunCrate"))
        {
            nearbyCrate = null;
            
        }
        if (collision.gameObject.CompareTag("door"))
        {
            nearbyDoor = null;

        }
        if (collision.gameObject.CompareTag("upgradePortal"))
        {
            nearbyPortal = null;
        }
       
    }
}

