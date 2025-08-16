using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class enemyManager : MonoBehaviour
{
    public Transform spawnTrans;
    public bool occupied;
    public GameObject enemy;
    public void OnTriggerStay2D(Collider2D collision)
    {
       
            occupied = true;
        
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        occupied = false;
    }

    // Update is called once per frame

    private void Start()
    {
        trySpawnAtLocation();
        trySpawnAtLocation();
    }
    private void Update()
    {
       
    }

    public bool trySpawnAtLocation()
    {
        if(!occupied)
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
