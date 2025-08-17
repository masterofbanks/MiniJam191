using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerStats : MonoBehaviour
{

    public int hp = 6;
    public int totalHP = 6;
    public int speed;
    public int activeGun;
    public float startHealDelay;
    public float healDelay;
    public GameManager gameManagerScript;
    bool tookDamage;
    public GameObject[] gunPrefabs;
    public GameObject player;
    public GameObject currentGun;
    
    public int gold = 0;

    public int gems = 0;


    private void Start()
    {
        player = GameObject.FindWithTag("playerCharacter");
        currentGun = GameObject.FindWithTag("gun");
        hp = totalHP;
        gameManagerScript = GameObject.Find("GameController").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (tookDamage)
        {
            StopAllCoroutines();
            StartCoroutine(startHealing());
            tookDamage = false;
        }
        
      

        gameManagerScript.goldText.text = "x" + gold.ToString();
        gameManagerScript.gemText.text = "x" + gems.ToString();




    }
   
    public void changeGun(int activeGun)
    {
  
       
            Destroy(currentGun);
        

        // Safety check: make sure the index exists
        if (activeGun >= 0 && activeGun < gunPrefabs.Length)
        {
           
            currentGun = Instantiate(gunPrefabs[activeGun], player.GetComponent<Transform>().position, Quaternion.identity, player.GetComponent<Transform>());

            Debug.Log("Equipped gun: " + activeGun);
        }
        else
        {
            currentGun = Instantiate(gunPrefabs[0], player.GetComponent<Transform>().position, Quaternion.identity);
        }
    }

    public static void ApplyDamage(GameObject Target, int damage = 0, int hpc = 0)
    {
        
        playerStats targetHealth = Target.GetComponent<playerStats>();
        if (targetHealth) targetHealth.healthUpdate(damage, hpc);
    }

    public  void healthUpdate(int damage =0, int healthChange=0)
    {
        hp -= damage;
        if(hp <= 0)
        {
            gameManagerScript.gameOver();
        }
        if (damage > 0)
        {
            tookDamage = true;
        }
        totalHP += healthChange;
        
        StartCoroutine(startHealing());
        
    }
    IEnumerator startHealing()
    {
        
        yield return new WaitForSeconds(startHealDelay);
       
        while (hp < totalHP)
        {
          
            yield return new WaitForSeconds(healDelay);
            hp++;
            
        }
    }

}
//when crystal ball triggered, set the players spell to be a instance of the spell class