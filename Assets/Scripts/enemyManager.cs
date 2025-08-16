using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyManager : MonoBehaviour
{
    public Transform spawnTrans;
    
    // Update is called once per frame
   
    public  void spawnAtLocation(GameObject enemy)
    {
        Object.Instantiate(enemy, spawnTrans, true);
    }
}
