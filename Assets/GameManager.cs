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
    public bool changeCam;
    public GameObject spawnerParent;
    public GameObject healthContainer;
    public GameObject heartIcon;
    public GameObject gruntPrefab;
    public GameObject Goldling;
    public GameObject Gemenie;

    public TextMeshProUGUI goldText;
    public TextMeshProUGUI gemText;

    public int waveAmount;
    public float rateOfSpawn = 0.55f; //how many enemies per second

    // Start is called before the first frame update
    void Start()
    {
        spawnerParent = GameObject.FindWithTag("spawners");
        inDrill = true;
        changeCam = false;
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
        if(Input.GetMouseButtonDown(1) && changeCam)
        {
            inDrill = !inDrill;
            drillCam.SetActive(inDrill);
            mapCam.SetActive(!inDrill);
        }

        UpdateHealth();

    }

    public void UpdateHealth()
    {
        int numHearts = GameObject.FindWithTag("playerCharacter").GetComponent<playerStats>().totalHP;
        int currentHealth = GameObject.FindWithTag("playerCharacter").GetComponent<playerStats>().hp;
        if(currentHealth >= 0)
        {
            for (int i = numHearts - 1; i >= currentHealth; i--)
            {
                healthContainer.transform.GetChild(i).GetComponent<Animator>().SetBool("empty", true);
            }
        }
        
    }

    public void gameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }




    public IEnumerator spawnWave(int amount, float ratePerSec, float typeRatioNormal, float typeRatioSpecial)
    {
        Debug.Log("01");
        float typeOf;
        int totalSpawnPoints = spawnerParent.transform.childCount;
        bool sucessfulspawn;
        GameObject enemy;
        for(int i = 0; i < amount; i++)
        {
            Debug.Log("1");
           sucessfulspawn = false;
            yield return new WaitForSeconds(1f / ratePerSec);
            while (!sucessfulspawn)
            {
                Debug.Log("2");
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
                        enemy = Gemenie;
                        break;
                    default:
                        enemy = gruntPrefab;
                        break;

                }
                

                int childNum = Random.Range(0, totalSpawnPoints);
                sucessfulspawn = spawnerParent.transform.GetChild(childNum).GetComponent<enemyManager>().trySpawnEnemy(enemy);
            }

        }

    }
    public IEnumerator spawnWave(int amount, float ratePerSec, int type)
    {

        
        int totalSpawnPoints = spawnerParent.transform.childCount;
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
                        enemy = Gemenie;
                        break;
                    default:
                        enemy = gruntPrefab;
                        break;

                }


                int childNum = Random.Range(0, totalSpawnPoints);
                sucessfulspawn = spawnerParent.transform.GetChild(childNum).GetComponent<enemyManager>().trySpawnEnemy(enemy);
            }

        }

    }

}
