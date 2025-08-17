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
    public void changeGun(int activeGun)
    {
        switch (activeGun)
        {
            case 0:
                //default pistol
                break;
            case 1:
                //stronger pistol
                break;
            case 2:
                //burst rifle
                break;
            case 3:
                //semi auto rifle
                break;
            case 4:
                //shotgun
                break;
            case 5:
                //auto rifle
                break;
            case 6:
                //smg
                break;
            case 7:
                //sniper
                break;
            case 8:
                //laser
                break;
            case 9:
                //mini gun
                break;
            case 10:
                //rocket launcher special rare (need to add explosion mechanic)
                break;
            default:
                //default pistol
                break;




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