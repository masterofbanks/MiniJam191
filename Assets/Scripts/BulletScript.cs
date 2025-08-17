using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCam;
    Rigidbody2D rb;
    public Transform playerTrans;

    public float force; //bullet speed
    public float bulletDamage;
    public float travelTime;
    public float accuracy; 

    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        GameObject meshObj = GameObject.Find("playerCharacterMesh");
        if (meshObj != null)
        {
            playerTrans = meshObj.transform;
        }
        else
        {
            Debug.LogWarning("playerCharacterMesh not found in scene!");
        }



        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        // accuracy is a value between 0 (worst) and 1 (perfect)
        float maxSpread = 80f;
        float spreadAngle = Random.Range(-maxSpread + (maxSpread * accuracy), maxSpread - (maxSpread * accuracy)); ; 

        Vector3 direction;
        if (Vector3.Distance(mousePos, transform.position) <= 10.05f)
        {
            direction = mousePos - playerTrans.position;
        }
        else
        {
            direction = mousePos - transform.position;
        }

        // Rotate the direction vector by the spread angle
        float angleRad = spreadAngle * Mathf.Deg2Rad;
        Vector2 rotatedDir = new Vector2(
            direction.x * Mathf.Cos(angleRad) - direction.y * Mathf.Sin(angleRad),
            direction.x * Mathf.Sin(angleRad) + direction.y * Mathf.Cos(angleRad)
        ).normalized;

        // Apply velocity
        rb.velocity = rotatedDir * force;

        // Rotate the bullet sprite to match movement direction
        float rot = Mathf.Atan2(rotatedDir.y, rotatedDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);



    }
    public void upgradeBulletStats(float bulletSpeed, float Range, float AD, float accu)
    {
        if (accu+accuracy > 1)
        {
            accuracy = 1;
        }
        else
        {
            accuracy = accuracy + accu;
        }
        force += bulletSpeed;
        travelTime += Range;
        bulletDamage += AD;
    }
    public void updateBulletStats(float bulletSpeed, float Range, float AD, float accu)
    {
        force = bulletSpeed;
        travelTime = Range;
        bulletDamage = AD;
        accuracy = accu;
       

    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > travelTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            
            Enemyscript.bulletHit(other.gameObject, bulletDamage);
        }

        else if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    
}
