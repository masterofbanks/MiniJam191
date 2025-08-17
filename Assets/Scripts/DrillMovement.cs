using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class DrillMovement : MonoBehaviour
{
    public float aimAngle;
    public Vector2 aimDirection;

    public GameObject arrowRotationPoint;
    public float drillSpeed;
    public bool inDeposit;
    private Rigidbody2D rb;
    public GameObject targetDeposit;
    public GameObject mineButton;
    public GameManager gameManagerScript;
    public TerrainGeneration terrainGenScript;
    public bool firstDirectionSet;
    public int depositWaveAmount;
    public float rarity;
    private Vector3 startPos;
    public shooting bullet;
    public depositParticleSpawner particleSpawner;
    public GameObject newAreaText;
    public GameObject hitSomethingText;
    public float blinkDuration = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        particleSpawner = GameObject.FindGameObjectWithTag("particleSpawner").GetComponent<depositParticleSpawner>();
        rb = GetComponent<Rigidbody2D>();
        inDeposit = false;
        startPos = transform.position;
        firstDirectionSet = false;
        bullet = GameObject.FindGameObjectWithTag("gun").GetComponent<shooting>();
    }

    // Update is called once per frame
    void Update()
    {
        SetCrosshairPosition();
        if (Input.GetMouseButtonDown(0) && ValidAngle(aimAngle) && !inDeposit && !gameManagerScript.inDrill && gameManagerScript.nearTerminal)
        {
            rb.velocity = aimDirection * drillSpeed;
            transform.rotation = Quaternion.Euler(0, 0, aimAngle + 90f);
            if (!firstDirectionSet)
            {
                firstDirectionSet = true;
                StartCoroutine(gameManagerScript.spawnWave(gameManagerScript.waveAmount, 1f, 0.95f, 0.95f));
            }
        }
            
        mineButton.SetActive(inDeposit);

    }

    private void FixedUpdate()
    {
        
    }

    private void SetCrosshairPosition()
    {

        //get position of the mouse in world coords
        var worldMousePosition =
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        //vector between player and mouse
        var facingDirection = worldMousePosition - transform.position;
        //get angle between facingDirection and horizontal axis
        aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
        //make sure its between 0 and 2pi
        if (aimAngle < 0f)
        {
            aimAngle = Mathf.PI * 2 + aimAngle;
        }


        aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;
        aimDirection.Normalize();
        //convert to degrees


        aimAngle = aimAngle * 180 / Mathf.PI;

        //rotate the player towards the mouse via the aimAngle

        if(ValidAngle(aimAngle))
        {
            arrowRotationPoint.transform.rotation = Quaternion.Euler(0, 0, aimAngle);
        }


        
    }

    bool ValidAngle(float angle)
    {
        return angle > 185f && angle < 355f;
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Deposit"))
        {
            int depositType = collision.gameObject.GetComponent<depositProperties>().depositTypeOf;

            if (depositType == 0)
            {
                StartCoroutine(gameManagerScript.spawnWave(depositWaveAmount, 0.55f, 1- rarity, rarity, 0));
                particleSpawner.spawnOre(depositType+4);
                hitSomethingText.GetComponent<TextMeshProUGUI>().text = "Hit a deposit!";
            }
            else if(depositType < 4)
            {
                StartCoroutine(gameManagerScript.spawnWave(depositWaveAmount, 0.55f, 0.05f, 0.05f, depositType - 1));
                particleSpawner.spawnOre(depositType);
                hitSomethingText.GetComponent<TextMeshProUGUI>().text = "Hit a deposit!";
            }

            else if(depositType == 4)
            {

                string[] upgrades = { "velocity", "range", "damage", "accuracy", "piercing"};
                float[] weights = { 0.3f, 0.3f, 0.10f, 0.25f, 0.05f };
                float roll = Random.Range(0f, 1f);
                float cumulative = 0f;
                for (int i = 0; i < upgrades.Length; i++)
                {
                    cumulative += weights[i];
                    if (roll <= cumulative)
                    {
                        
                        switch (upgrades[i])
                        {
                            case "velocity":
                                if (bullet.piercing)
                                    bullet.upgradeBulletStats(bullet.gunVelocity*0.25f, 0, 0, 0, true, 0);
                                else
                                {
                                    bullet.upgradeBulletStats(5, 0, 0, 0, false, 0);
                                }
                                break;
                            case "range":
                                if (bullet.piercing)
                                    bullet.upgradeBulletStats(0, bullet.gunBulletRange*0.25f, 0, 0, true, 0);
                                else
                                {
                                    bullet.upgradeBulletStats(0, 1, 0, 0, false, 0);
                                }
                                break;
                            case "damage":
                                if (bullet.piercing)
                                    bullet.upgradeBulletStats(0, 0, bullet.gunDamage * 0.25f, 0, true, 0);
                                else
                                {
                                    bullet.upgradeBulletStats(0, 0, 5, 0, false, 0);
                                }
                                break;
                            case "accuracy":
                                if (bullet.piercing)
                                    bullet.upgradeBulletStats(0, 0, 0, bullet.gunAccuracy * 0.25f, true, 0);
                                else
                                {
                                    bullet.upgradeBulletStats(0, 0, 0, 0.05f, false, 0);
                                }
                               
                                break;
                            case "piercing":
                                if(bullet.piercing)
                                bullet.upgradeBulletStats(0, 0, 0, 0, true, bullet.maxDamage*0.25f);
                                else
                                {
                                    bullet.upgradeBulletStats(0, 0, 0, 0, true, 10);
                                }
                                break;
                                
                        }
                        Debug.Log(upgrades[i]);
                        break;
                    }
                }

                hitSomethingText.GetComponent<TextMeshProUGUI>().text = "Upgrade Hit!";
            }

            inDeposit = true;
            targetDeposit = collision.gameObject;
            StartCoroutine(blinkText(hitSomethingText));
            Mine();
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            ResetPosition();
            StartCoroutine(blinkText(newAreaText));
        }

    }


    public void Mine()
    {
        if(targetDeposit != null)
            Destroy(targetDeposit);
            inDeposit = false;
    }

    public void ResetPosition()
    {
        int randAmount = UnityEngine.Random.Range(depositWaveAmount - (int)Mathf.Floor(depositWaveAmount / 2), depositWaveAmount + (int)Mathf.Floor(depositWaveAmount / 2));
        depositWaveAmount = randAmount;
        float randRarity = UnityEngine.Random.Range(0.8f, 0.999f);
        rarity = randRarity;
        rb.velocity = drillSpeed * Vector2.down;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = startPos;
        terrainGenScript.NewGeneration();
    }

    IEnumerator blinkText(GameObject obj)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(blinkDuration);
        obj.SetActive(false);
    }
}
