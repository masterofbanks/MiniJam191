
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    [Header("Terrain Generation Settings")]
    public float startingScale;
    public float offsetX;
    public float offsetY;
    public int minNumberOfDeposits;

    [Header("Deposits")]
    public GameObject goldDeposit;
    public GameObject rareOreDeposit;
    public GameObject[] oreTypes;
    public GameObject upgrade;

    [Header("Deposit Frequencies")]
    public float rareSpawnRateScaler;

    private BoxCollider2D terrainCollider;
    private float scale;
    private int currentNumberOfDeposits;
    public int currentNumberOfRareDeposits;
    public int numupgrades;

    private void Start()
    {
        scale = startingScale;
        terrainCollider = GetComponent<BoxCollider2D>();
        NewGeneration();
    }

    public void NewGeneration()
    {
        scale = startingScale;
        DestroyDeposits();
        int randIndex = UnityEngine.Random.Range(0, oreTypes.Length);
        rareOreDeposit = oreTypes[randIndex];

        while (currentNumberOfDeposits < minNumberOfDeposits)
        {
            DestroyDeposits();
            offsetX = UnityEngine.Random.Range(0f, 99999f);
            offsetY = UnityEngine.Random.Range(0f, 99999f);
            GenerateTerrain();

            scale += 10;

        }

        if (currentNumberOfRareDeposits == 0)
        {
            StartCoroutine(CorrectRareDeposit(rareOreDeposit));

        }

        if (numupgrades == 0)
        {
            StartCoroutine(CorrectRareDeposit(upgrade));
        }



    }

    IEnumerator CorrectRareDeposit(GameObject replacement)
    {
        yield return new WaitForSeconds(0.01f);
        GameObject[] deposits = GameObject.FindGameObjectsWithTag("Deposit");
        GameObject randomDeposit = deposits[UnityEngine.Random.Range(0, deposits.Length)];

        Vector3 randomPos = randomDeposit.transform.position;
        Destroy(randomDeposit);
        Instantiate(replacement, randomPos, Quaternion.identity);
        currentNumberOfRareDeposits++;
    }

    

    private void DestroyDeposits()
    {
        currentNumberOfDeposits = 0;
        currentNumberOfRareDeposits = 0;
        GameObject[] deposits = GameObject.FindGameObjectsWithTag("Deposit");
        foreach (GameObject deposit in deposits)
        {
            Destroy(deposit);
        }
    }

    /* 
         1. Instead of Using texture data, use a box collider
         2. Translate the 4 Box Coordinates into Pixel Coordinates
         3. Use those pixel coordinates as bounds for the for loops of perlin noise generation
         4. Based off of each pixel's coorindates, calculate the perlin noise value
         5. Based off of perlin noise values, set World Position Instantation of a certain prefab
    */
    void GenerateTerrain()
    {

        Vector3 lowerLeftWorld = terrainCollider.bounds.min;
        Vector3 upperRightWorld = terrainCollider.bounds.max;

        Vector3 lowerLeftPixel = Camera.main.WorldToScreenPoint(lowerLeftWorld);
        Vector3 upperRightPixel = Camera.main.WorldToScreenPoint(upperRightWorld);

        //Generate Perlin Noise Map For Texture
        for (int x = (int)lowerLeftPixel.x; x < (int)upperRightPixel.x; x++)
        {
            for (int y = (int)lowerLeftPixel.y; y < (int)upperRightPixel.y; y++)
            {
                float xCoord = (float)x / (int)upperRightPixel.x;
                float yCoord = (float)y / (int)upperRightPixel.y;
                float perlinNumber = CalculatePerlin(xCoord, yCoord);
                if (perlinNumber > 0.99f)
                {
                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(x, y, lowerLeftPixel.z));
                    if(OutDepositBounds(worldPos))
                    {
                        float normalizedYPos = (float)(y - (int)lowerLeftPixel.y) / (int)upperRightPixel.y;
                        PutDeposit(worldPos, CalculateRareSpawnRate(normalizedYPos));
                        currentNumberOfDeposits++;
                    }
                }   
            }
        }

        

        

    }

    float CalculatePerlin(float x, float y)
    {
        float xCoord = x * scale + offsetX;
        float yCoord = y * scale + offsetY;
        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        return sample;
    }

    bool OutDepositBounds(Vector3 pos)
    {
        GameObject[] deposits = GameObject.FindGameObjectsWithTag("Deposit");
        foreach (GameObject deposit in deposits)
        {
            if (deposit.GetComponent<CircleCollider2D>().bounds.Contains(pos))
            {
                return false;
            }
        }
        return true;
    }

    void PutDeposit(Vector3 pos, float rareSpawnRate)
    {
        float randomValue = UnityEngine.Random.Range(0f, 1f);

        if(randomValue < rareSpawnRate)
        {
            Instantiate(rareOreDeposit, pos, Quaternion.identity);
            currentNumberOfRareDeposits++;
        }

        else if(randomValue >= rareSpawnRate && randomValue < 0.9f)
        {
            Instantiate(goldDeposit, pos, Quaternion.identity);
        }
        else
        {
            Instantiate(upgrade, pos, Quaternion.identity);
        }


    }

    float CalculateRareSpawnRate(float yLevel)
    {
        return Mathf.Exp(-yLevel/ rareSpawnRateScaler);
    }


}
