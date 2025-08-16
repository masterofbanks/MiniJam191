using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemyscript : MonoBehaviour
{

    NavMeshAgent agent;


    [SerializeField] Transform targetTransform;

    public int damageAmount = 1;
    public int slapSpeed;
   
    public bool slapping =true;
    public bool inRange = true;
    GameObject crack;

    public float enemyHealth;
    //gameObject.GetComponent<NavMeshAgent>().isStopped = true/false;
    void Awake()
    {

        
    }

    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        GameObject targetAcquired = GameObject.Find("playerCharacter");
        targetTransform = targetAcquired.transform;

        
        
    }

    // Update is called once per frame
    public void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log("1");
        // Print the object name
        Debug.Log("Collided with: " + collision.gameObject.name);

        // Print the full path in hierarchy (sometimes useful for children)
        Debug.Log("Collided root: " + collision.transform.root.name);
        if (collision.gameObject.CompareTag("playerCharacter"))
        {
            Debug.Log("2");
            crack = collision.gameObject;

            inRange = true;

        }

    }
    void Update()
    {
        Vector3 targetPosition = targetTransform.position;
        
       

        agent.SetDestination(targetPosition);

        if(inRange&& !slapping)
        {
            StartCoroutine(slapDelay(crack));
            slapping = true;
        }


        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
            playerStats.points +=60;
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
        if (slapping && inRange)
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
        enemyHealth -= damag;
        playerStats.points +=10;
        Debug.Log(playerStats.points);
    }
}
