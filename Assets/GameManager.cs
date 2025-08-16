using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool inDrill; //false if looking at the map

    public GameObject mapCam;
    public GameObject drillCam;

    // Start is called before the first frame update
    void Start()
    {
        inDrill = true;
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
        
    }
}
