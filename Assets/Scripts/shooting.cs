using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class shooting : MonoBehaviour
{
    private Camera mainCam;

    public float aimAngle;
    public Vector2 aimDirection;
    public GameObject bullet;
    public Transform bulletTransform; //where the bullet spawns
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

    }

    // Update is called once per frame
    void Update()
    {
        bullet.GetComponent<BulletScript>().updateBulletStats(gunVelocity, gunBulletRange, gunDamage, gunAccuracy, piercing, maxDamage);
        SetCrosshairPosition();

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

        for (int i = 0; i < burstAmount; i++)
        {

            Vector3 bulletTrans = bulletTransform.position;
            GameObject bulletObj = Instantiate(bullet, bulletTrans, rot);
            bulletObj.GetComponent<BulletScript>().direction = aimDirection;
            yield return new WaitForSeconds(burstSpeed);
        }

        yield return new WaitForSeconds(0.1f);
     
    }

    private void SetCrosshairPosition()
    {

        //get position of the mouse in world coords
        var worldMousePosition =
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        //vector between player and mouse
        var facingDirection = worldMousePosition - transform.position;
        //get angle between facingDirection and horizontal axis
        aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
        //make sure its between 0 and 2pi
        if (aimAngle < 0f)
        {
            aimAngle = Mathf.PI * 2 + aimAngle;
        }


        aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;
        aimDirection.Normalize();
        //convert to degrees


        aimAngle = aimAngle * 180 / Mathf.PI;

        //rotate the player towards the mouse via the aimAngle

        transform.rotation = Quaternion.Euler(0, 0, aimAngle);
    }
}
