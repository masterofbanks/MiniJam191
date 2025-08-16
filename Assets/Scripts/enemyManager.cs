using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class enemyManager : MonoBehaviour
{
    public Transform spawnTrans;
    public bool occupied;
    public GameObject enemy;
    public float spawnInterval = 0.5f;
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
        InvokeRepeating(nameof(trySpawnAtLocation), 0f, spawnInterval);
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
