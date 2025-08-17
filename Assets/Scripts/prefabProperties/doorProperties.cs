using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorProperties : MonoBehaviour
{
    public int doorPrice;
    public GameObject[] spawners;

    public void turnSpawnersOn()
    {
        for(int i = 0; i < spawners.Length; i++)
        {
            spawners[i].tag = "onSpawners";
        }
    }

}
