using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class enemyManager : MonoBehaviour
{
    public Transform spawnTrans;
    public float timeBetweenEnemySpawns;
    public GameObject enemy;
    public bool occupied;
    public DrillMovement drillMovementScript;
    public float healthScaler = 2f; // Scales the enemy health based on the drill area  


    // Update is called once per frame
    public void OnTriggerStay2D(Collider2D collision)
    {

        occupied = true;

    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        occupied = false;
    }
    private void Start()
    {
        drillMovementScript = GameObject.FindGameObjectWithTag("drill").GetComponent<DrillMovement>();
    }
    

    public bool trySpawnEnemy(GameObject enemy)
    {
       
        if (!occupied)
        {

            enemy = Object.Instantiate(enemy, spawnTrans);
            enemy.GetComponent<Enemyscript>().enemyHealth += healthScaler * drillMovementScript.numArea;
            return true;
        }

        else
        {
            return false;
        }
        
        
    }

}
