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

    }
    private void Update()
    {
       
    }

    public bool trySpawnEnemy(GameObject enemy)
    {
       
        if (!occupied)
        {

            Object.Instantiate(enemy, spawnTrans);
            return true;
        }

        else
        {
            return false;
        }
        
        
    }

}
