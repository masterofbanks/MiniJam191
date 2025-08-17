using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemyscript : MonoBehaviour
{

    NavMeshAgent agent;


    

    public int damageAmount = 1;
    public float slapSpeed;
    public int enemyType;
    public bool slapping =false;
    public bool inRange = false;
    GameObject crack;
    public GameManager gameManagerScript;
    public GameObject player;
    
    public float enemyHealth;
    //gameObject.GetComponent<NavMeshAgent>().isStopped = true/false;
    void Awake()
    {

        
    }

    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("playerCharacter");
        if (player == null)
        {
            Debug.Log("Player not found! Check the tag is correct.");
            
        }else
        {
            Debug.Log("Player found!");
        }
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
        

        
        
    }

    // Update is called once per frame
    public void OnTriggerEnter2D(Collider2D collision)
    {

        // Print the object name

        // Print the full path in hierarchy (sometimes useful for children)
        if (collision.gameObject.CompareTag("playerCharacter"))
        {
            crack = collision.gameObject;

            inRange = true;

        }

    }
    void Update()
    {
        Vector3 targetPosition = player.transform.position;
        
       

        agent.SetDestination(targetPosition);

        if(inRange&& !slapping)
        {
            StartCoroutine(slapDelay(crack));
            slapping = true;
        }


        if (enemyHealth <= 0)
        {
            switch (enemyType)
            {
                case -1:
                    player.GetComponent<playerStats>().gold += 20;
                    break;

                case 0:
                    player.GetComponent<playerStats>().gold += 60;
                    break;
                case 1:
                    player.GetComponent<playerStats>().gems += 30;
                    break;


            }
            player.GetComponent<playerStats>().enemyKillCount++;
            Destroy(gameObject);
            
        }
    }

   
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("playerCharacter"))
        {
            inRange = false;
            slapping = false;
        }
    }
    private void sendDamage(GameObject reciever, int recievingDamage)
    {
        playerStats.ApplyDamage(reciever, recievingDamage);
    }

    IEnumerator slapDelay(GameObject other)
    {
        yield return new WaitForSeconds(slapSpeed);
        if (inRange)
        {
            sendDamage(other, damageAmount);
            slapping = false;
        }
    }



    public static void bulletHit(GameObject Target, float damage)
    {
        Enemyscript targetHealth = Target.GetComponent<Enemyscript>();
        if (targetHealth) targetHealth.enemyTakeDamage(damage);
    }
    private void enemyTakeDamage(float damag)
    {
        if (gameManagerScript == null)
            gameManagerScript = GameObject.FindObjectOfType<GameManager>();
        switch (enemyType)
        {
            case -1:
                enemyHealth -= damag;
                float roll = Random.Range(0f, 1f);
                if (roll < Mathf.Pow(0.95f, gameManagerScript.planet))
                    player.GetComponent<playerStats>().gold += 10;
                else
                    player.GetComponent<playerStats>().gems += 10;


                break;
            case 0:
                enemyHealth -= damag;
                player.GetComponent<playerStats>().gold += 30;
                break;
            case 1:
                enemyHealth -= damag;
                player.GetComponent<playerStats>().gems += (int)Mathf.Pow(30, enemyType);
                break;

        }
    }
}
