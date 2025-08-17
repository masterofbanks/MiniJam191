using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class depositParticleSpawner : MonoBehaviour
{
    public GameObject[] oreprefabs;
    public float spawnRate = 0.1f;
    public float minSpeed = 1f;
    public float maxSpeed = 5f;
    public float minScaleSpeed = 0.2f;
    public float maxScaleSpeed = 1f;
    public float spawnRadius = 0.1f;

    private Camera maincam;
    private bool spawning = false;

    [Header("Ore Type")]
    public int typeofore = 0;

    void Start()
    {
        //sdfsdfsdf
        maincam = Camera.main;
        startParticleEffect();
    }

    public void startParticleEffect()
    {
        if (!spawning)
        {
            spawning = true;
            StartCoroutine(SpawnRoutine());
        }
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            spawnOre(typeofore);
            yield return new WaitForSeconds(spawnRate);
        }
    }

    public void spawnOre(int type)
    {
        typeofore = type;

        Vector3 spawnPos = transform.position + (Vector3)(Random.insideUnitCircle * spawnRadius);
        GameObject ore = Instantiate(
            oreprefabs[typeofore],
            spawnPos,
            Quaternion.Euler(0, 0, Random.Range(0f, 360f))
        );

        float speed = Random.Range(minSpeed, maxSpeed);
        float scaleSpeed = Random.Range(minScaleSpeed, maxScaleSpeed);

        depositParticle particle = ore.AddComponent<depositParticle>();
        particle.direction = Random.insideUnitCircle.normalized;
        particle.speed = speed;
        particle.scaleSpeed = scaleSpeed;
    }

    // Call this to change ore temporarily
    public void SetOreTemporarily(int type, float duration)
    {
        StopCoroutine("ResetOreType"); // cancel any previous reset
        typeofore = type;
        StartCoroutine(ResetOreType(duration));
    }

    private IEnumerator ResetOreType(float delay)
    {
        yield return new WaitForSeconds(delay);
        typeofore = 0; // reset back to default
    }
}
