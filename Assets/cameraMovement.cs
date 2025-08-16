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
    float x=0;
    float y=0;
    private void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        
        
    }


    void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        camDist = Vector3.Distance(mousePos, camTrans.position);
        Debug.Log(camDist);
        //doesnt work yet
        Vector3 camDirection = Vector3.Lerp(camTrans.position, mousePos, camDist);

      
            x = Mathf.Clamp(camDirection.x,0f, camPushMax);
            y = Mathf.Clamp(camDirection.y, 0f, camPushMax);
        
        transform.position = new Vector3(playerTrans.position.x+x, playerTrans.position.y+y, -10);
      /* if (Vector3.Distance(mousePos, camTrans.position) >5)
        {

        }*/

    }
}
