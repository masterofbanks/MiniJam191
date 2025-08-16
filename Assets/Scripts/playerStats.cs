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
    
    bool tookDamage;

   
    

   [SerializeField] public static int points = 0;

    private void Start()
    {
        hp = totalHP;
    }

    private void Update()
    {
        if (tookDamage)
        {
            StopAllCoroutines();
            
            StartCoroutine(startHealing());
           
            tookDamage = false;
        }

      


    }
    public static void ApplyDamage(GameObject Target, int damage = 0, int hpc = 0)
    {
        
        playerStats targetHealth = Target.GetComponent<playerStats>();
        if (targetHealth) targetHealth.healthUpdate(damage, hpc);
    }

    public  void healthUpdate(int damage, int healthChange)
    {
        hp -= damage;
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
