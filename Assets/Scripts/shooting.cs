using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : MonoBehaviour
{
    private Camera mainCam;
    Vector3 mousepos;
    public Transform playerTrans;
    public Transform gunTrans;
    public Transform gunEndTrans;
    public GameObject bullet;
    public Transform bulletTransform;
    public bool canFire;
    private float timer;
    
    
    [Header("gun stats")]
    public bool Semi;
    public float timeBetweenFiring; //firerate
    public float gunDamage;
    public float gunAccuracy;
    public float gunBulletRange;
    public float gunVelocity;

    /*public void updategunStats(float fireRate,float bulletSpeed, float Range, float AD, float accu, bool isSemi)
    {
        timeBetweenFiring = fireRate;
        bullet.GetComponent<BulletScript>().updateBulletStats(bulletSpeed, Range, AD, accu);
        Semi = isSemi;
    }*/


    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
        mousepos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        if(Vector3.Distance(mousepos, gunTrans.position)<=10.12f)
        {
            Vector3 rotation = mousepos - playerTrans.position;
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotZ - 90);
            Vector2 scale = transform.localScale;
            //flips player

            if (playerTrans.position.x - gunEndTrans.position.x > 0)
            {

                rotation.z = rotation.z * -1;
            }
            else if (playerTrans.position.x - gunEndTrans.position.x < 0)
            {

                rotation.z = rotation.z * -1;
            }
            transform.localScale = scale;
        }
        else
        {
            Vector3 rotation = mousepos - gunEndTrans.position;
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotZ - 90);
            Vector2 scale = transform.localScale;
            //flips player

            if (playerTrans.position.x - gunEndTrans.position.x > 0)
            {

                rotation.z = rotation.z * -1;
            }
            else if (playerTrans.position.x - gunEndTrans.position.x < 0)
            {

                rotation.z = rotation.z * -1;
            }
            transform.localScale = scale;
        }

        if (!canFire)
        {
            timer += Time.deltaTime;
            if(timer > timeBetweenFiring)
            {
                canFire = true;
                timer = 0;
            }
        }

        if (Input.GetMouseButton(0) && canFire &&!Semi)
        {
            canFire = false;
            Instantiate(bullet, bulletTransform.position, Quaternion.identity);

        }else if(Input.GetMouseButtonDown(0) && canFire)
        {
            canFire = false;
            Instantiate(bullet, bulletTransform.position, Quaternion.identity);
        }

      
    }
}
