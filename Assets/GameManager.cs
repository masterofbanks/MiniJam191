using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool inDrill; //false if looking at the map

    public GameObject mapCam;
    public GameObject drillCam;

    public GameObject healthContainer;
    public GameObject heartIcon;

    // Start is called before the first frame update
    void Start()
    {
        inDrill = true;
        int numHearts = GameObject.FindWithTag("playerCharacter").GetComponent<playerStats>().totalHP;
        for(int i = 0; i < numHearts; i++)
        {
            Instantiate(heartIcon, new Vector3(0, 0, 0), Quaternion.identity, healthContainer.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
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
        for(int i = numHearts - 1; i > currentHealth; i--)
        {
            healthContainer.transform.GetChild(i).GetComponent<Animator>().SetBool("empty", true);
        }
    }
}
