using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inDeposit = false;
        startPos = transform.position;
        firstDirectionSet = false;
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
                
            }
            else
            {
                StartCoroutine(gameManagerScript.spawnWave(depositWaveAmount, 0.55f, 0.05f, 0.05f, depositType - 1));

            }

            inDeposit = true;
            targetDeposit = collision.gameObject;
            Mine();
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            ResetPosition();

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
        transform.position = startPos;
        terrainGenScript.NewGeneration();
    }
}
