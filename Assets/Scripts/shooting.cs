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
    public bool piercing;
    public bool Semi;
    public int burstAmount;
    public float burstspeed;
    public float timeBetweenFiring; //firerate
    public float gunDamage;
    public float gunAccuracy;
    public float gunBulletRange;
    public float gunVelocity;
    public float maxDamage;
    /*public void updategunStats(float fireRate,float bulletSpeed, float Range, float AD, float accu, bool isSemi)
    {
        timeBetweenFiring = fireRate;
        bullet.GetComponent<BulletScript>().updateBulletStats(bulletSpeed, Range, AD, accu);
        Semi = isSemi;
    }*/


    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        playerTrans = GameObject.FindWithTag("playerCharacterMesh").transform;
    }

    // Update is called once per frame
    void Update()
    {
        bullet.GetComponent<BulletScript>().updateBulletStats(gunVelocity, gunBulletRange, gunDamage, gunAccuracy, piercing, maxDamage);
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

        if (!canFire )
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
            StartCoroutine(fireGun(bullet, Quaternion.identity, burstspeed, burstAmount));

        }
        else if(Input.GetMouseButtonDown(0) && canFire)
        {
            canFire = false;
            StartCoroutine(fireGun(bullet, Quaternion.identity, burstspeed, burstAmount));
        }

      
    }
   
    public IEnumerator fireGun(GameObject bullet, Quaternion rot, float burstSpeed, int burstAmount)
    {
        
        for(int i = 0; i< burstAmount; i++)
        {
            
            Vector3 bulletTrans = bulletTransform.position;
            Instantiate(bullet, bulletTrans, rot);
            yield return new WaitForSeconds(burstSpeed);
        }
     
    }
}
