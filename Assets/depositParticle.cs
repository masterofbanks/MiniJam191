using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class depositParticle : MonoBehaviour
{
    public Vector3 direction;
    public float speed = 2f;
    public float scaleSpeed = 0.5f;
    public float lifeTime = 8f;
    public float timer = 0f;

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        transform.localScale += Vector3.one * scaleSpeed * Time.deltaTime;
        timer += Time.deltaTime;
        if (timer>=lifeTime)
        {
            Destroy(gameObject);
        }


    }
}
