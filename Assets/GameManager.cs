using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool inDrill; //false if looking at the map
    public int planet;

    public GameObject mapCam;
    public GameObject drillCam;
    public bool nearTerminal;
    public GameObject spawnerParent;
    public GameObject healthContainer;
    public GameObject heartIcon;
    public GameObject gruntPrefab;
    public GameObject Goldling;
    public GameObject[] Gemenies;

    public TextMeshProUGUI goldText;
    public TextMeshProUGUI gemText;
    public DrillMovement drillMovementScript;
    public GameObject explosion;
    public bool dead;

    [Header("Wave Settings")]
    public int startingWaveAmount;
    public int finishingWaveAmount;
    public int waveAmount = 0;
    public float rateOfSpawn = 0.55f; //how many enemies per second
    public float startTimeBetweenWaves;
    public float endTimeBetweenWaves;
    public float waveTimeScaler; //make this bigger to make timeBetweenWaves decrease slower
    public float waveAmountScaler; //make this bigger to make waveAmount increase slower
    

    private float localTimeBetweenWave = 0f;
    private float timeBetweenWaves;

    private float decimalWaveAmount;
    
    private float t = 0f;

    // Start is called before the first frame update
    void Start()
    {
        spawnerParent = GameObject.FindWithTag("spawners");
        inDrill = true;
        nearTerminal = false;
        waveAmount = startingWaveAmount;
        timeBetweenWaves = startTimeBetweenWaves;
        int numHearts = GameObject.FindWithTag("playerCharacter").GetComponent<playerStats>().totalHP;
        for(int i = 0; i < numHearts; i++)
        {
            Instantiate(heartIcon, new Vector3(0, 0, 0), Quaternion.identity, healthContainer.transform);
        }
        //StartCoroutine(spawnWave(15, 1f, 0.95f, 0.95f));

    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            if (drillMovementScript.firstDirectionSet)
            {
                t += Time.deltaTime;
                UpdateTimeBetweenWaves(t);
                UpdateWaveAmount(t);
                localTimeBetweenWave += Time.deltaTime;
            }
            int currentKillCount = GameObject.FindWithTag("playerCharacter").GetComponent<playerStats>().enemyKillCount;
            if (localTimeBetweenWave >= timeBetweenWaves && currentKillCount >= waveAmount)
            {
                localTimeBetweenWave = 0f;
                StartCoroutine(spawnWave(waveAmount, rateOfSpawn, 0.95f, 0.95f));
            }

            if (Input.GetMouseButtonDown(1) && nearTerminal)
            {
                FlipCams();

                GameObject.FindWithTag("playerCharacter").GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }


            UpdateHealth();
            UpdateHealth();
        }

    }

    public void FlipCams()
    {
        inDrill = !inDrill;
        drillCam.SetActive(inDrill);
        mapCam.SetActive(!inDrill);
    }

    public void GoBackToMainCam()
    {
        inDrill = true;
        drillCam.SetActive(true);
        mapCam.SetActive(false);
    }

    public void UpdateTimeBetweenWaves(float t)
    {
        // c exp(-t/a) + b
        // c + b = startTimeBetweenWaves;
        // b is the endTimeBetweenWaves;
        float c = startTimeBetweenWaves - endTimeBetweenWaves;
        timeBetweenWaves = c * Mathf.Exp(-t / waveTimeScaler) + endTimeBetweenWaves;
    }

    public void UpdateWaveAmount(float t)
    {
        // f(1 - exp(-x/waveAmountScaler) + startingWaveAmount

        decimalWaveAmount = (finishingWaveAmount - startingWaveAmount) * (1 - Mathf.Exp(-t / waveAmountScaler)) + startingWaveAmount;
        waveAmount = Mathf.FloorToInt(decimalWaveAmount);
    }

    public void UpdateHealth()
    {
        int numHearts = GameObject.FindWithTag("playerCharacter").GetComponent<playerStats>().totalHP;
        int currentHealth = GameObject.FindWithTag("playerCharacter").GetComponent<playerStats>().hp;

        for(int i = 0; i < numHearts; i++)
        {
            if(i < currentHealth)
            {
                healthContainer.transform.GetChild(i).GetComponent<Animator>().SetBool("empty", false);
            }
            else
            {
                healthContainer.transform.GetChild(i).GetComponent<Animator>().SetBool("empty", true);
            }
        }
        
    }

    public void gameOver()
    {
        SceneManager.LoadScene(0);
    }

    




    public IEnumerator spawnWave(int amount, float ratePerSec, float typeRatioNormal, float typeRatioSpecial, int gemType = 0)
    {
        float typeOf;
        int totalSpawnPoints = GameObject.FindGameObjectsWithTag("onSpawners").Length;
        GameObject.FindWithTag("playerCharacter").GetComponent<playerStats>().enemyKillCount = 0;
        bool sucessfulspawn;
        GameObject enemy;
        for(int i = 0; i < amount; i++)
        {
           sucessfulspawn = false;
            yield return new WaitForSeconds(1f / ratePerSec);
            while (!sucessfulspawn)
            {
                float roll = Random.Range(0f, 1f);

                if (roll < typeRatioNormal)
                {
                    typeOf = -1;
                }
                else
                {
                    roll = Random.Range(0f, 1f);
                    if (roll < typeRatioSpecial)
                    {
                        typeOf = 0;
                    }else
                    {
                        typeOf = 1;
                    }
                }
                switch(typeOf)
                {
                    case -1:
                        enemy = gruntPrefab;
                        break;

                    case 0:
                        enemy = Goldling;
                        break;

                    case 1:
                        enemy = Gemenies[gemType];
                        Debug.Log("gemType: " + gemType);
                        Debug.Log(Gemenies[gemType].name);
                        break;
                    default:
                        enemy = gruntPrefab;
                        break;

                }


                int childNum = Random.Range(0, totalSpawnPoints);
                sucessfulspawn = GameObject.FindGameObjectsWithTag("onSpawners")[childNum].GetComponent<enemyManager>().trySpawnEnemy(enemy);
            }

        }

    }
    public IEnumerator spawnWave(int amount, float ratePerSec, int type, int gemType = 1)
    {

        
        int totalSpawnPoints = GameObject.FindGameObjectsWithTag("onSpawners").Length;
        bool sucessfulspawn;
        GameObject enemy;
        for (int i = 0; i < amount; i++)
        {

            sucessfulspawn = false;
            yield return new WaitForSeconds(1f / ratePerSec);
            while (!sucessfulspawn)
            {
                
                switch (type)
                {
                    case -1:
                        enemy = gruntPrefab;
                        break;

                    case 0:
                        enemy = Goldling;
                        break;

                    case 1:
                        enemy = Gemenies[gemType];
                        break;
                    default:
                        enemy = gruntPrefab;
                        break;

                }
                

                int childNum = Random.Range(0, totalSpawnPoints);
                sucessfulspawn = GameObject.FindGameObjectsWithTag("onSpawners")[childNum].GetComponent<enemyManager>().trySpawnEnemy(enemy);
            }

        }

    }

}
