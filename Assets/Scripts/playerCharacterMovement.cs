using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterMovement : MonoBehaviour
{
    [Tooltip("A float value determining horizontal movement")]
    float hori;
    [Tooltip("A float value determining vertical movement")]
    float verti;
    [Tooltip("A float value determining the speed of movement")]
    public float speed;
    public float sprint;
    public float walking;


    public float stamina;
    public float maxStamina;
    public bool exausted;
    public float staminaRegen = 0.001f;
    public float staminaExaust = 0.003f;


    private Rigidbody2D rb;
    private void Start()
    {
        stamina = maxStamina;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        hori = Input.GetAxisRaw("Horizontal");
        verti = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(hori * speed, verti * speed);
        if (Input.GetKey("left shift") && stamina > 0)
        {
            if (!exausted)
            {
                speed = sprint;
                stamina -= staminaExaust;
            }

        }
        else
        {
            speed = walking;

        }
        if (stamina < maxStamina)
        {
            stamina += staminaRegen;
        }
        if (stamina <= 0)
        {
            exausted = true;

        }
        if (stamina >= maxStamina)
        {
            exausted = false;
        }
    }
    void Update()
    {
        //recieves wasd as input assiging a value from -1 to 1
        


        ////uses change in time, a preset speed variable, and input to move character
        //transform.Translate(Time.deltaTime * Vector2.up * speed * verti);
        //transform.Translate(Time.deltaTime * Vector2.right * speed * hori);

        

        

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("asscakses");
    }
}

