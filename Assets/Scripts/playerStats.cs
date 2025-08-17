using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerStats : MonoBehaviour
{

    public int hp = 6;
    public int totalHP = 6;
    public int speed;
    public float startHealDelay;
    public float healDelay;
    public GameManager gameManagerScript;
    bool tookDamage;

   
    
    public int gold = 0;

    public int gems = 0;


    private void Start()
    {
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
        Debug.Log("initial delay begin");
        yield return new WaitForSeconds(startHealDelay);
        Debug.Log("initial delay over");
        while (hp < totalHP)
        {
            Debug.Log("you are damaged healing now");
            yield return new WaitForSeconds(healDelay);
            hp++;
            Debug.Log("you have been healed one hp");
        }
    }

}
//when crystal ball triggered, set the players spell to be a instance of the spell class