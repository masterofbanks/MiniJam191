using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCam;
    public Transform camTrans;
    public Transform playerTrans;
    public float camPushMax;
    public float camDist;
 
    private void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        
        
    }

    private void FixedUpdate()
    {
        if (!GameObject.FindWithTag("GameController").GetComponent<GameManager>().dead)
        {
            mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

            Vector3 offset = mousePos - playerTrans.position;
            if (offset.magnitude > camPushMax)
            {
                offset = offset.normalized * camPushMax;
            }
            transform.position = new Vector3(playerTrans.position.x + offset.x, playerTrans.position.y + offset.y, -10f);
        }
    }

    void Update()
    {
        
    
   

    }
}
