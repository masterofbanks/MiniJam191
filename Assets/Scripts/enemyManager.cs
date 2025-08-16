using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class enemyManager : MonoBehaviour
{
    public Transform spawnTrans;
    public float timeBetweenEnemySpawns;
    public GameObject enemy;

    public float t = 0f;
    // Update is called once per frame

    private void Start()
    {

    }
    private void Update()
    {
        t += Time.deltaTime;
        if (t >= timeBetweenEnemySpawns)
        {
            t = 0f;
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemy, spawnTrans.position, Quaternion.identity);

    }

}
