using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DrillMovement : MonoBehaviour
{
    public float aimAngle;
    public Vector2 aimDirection;

    public GameObject arrowRotationPoint;
    public float drillSpeed;
    public bool inDeposit;
    private Rigidbody2D rb;
    public GameObject targetDeposit;
    public GameObject mineButton;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inDeposit = false;
    }

    // Update is called once per frame
    void Update()
    {
        SetCrosshairPosition();
        if (Input.GetMouseButtonDown(0) && ValidAngle(aimAngle) && !inDeposit)
        {
            rb.velocity = aimDirection * drillSpeed;
            transform.rotation = Quaternion.Euler(0, 0, aimAngle + 90f);
        }
            
        mineButton.SetActive(inDeposit);

    }

    private void FixedUpdate()
    {
        
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

        if(ValidAngle(aimAngle))
        {
            arrowRotationPoint.transform.rotation = Quaternion.Euler(0, 0, aimAngle);
        }


        
    }

    bool ValidAngle(float angle)
    {
        return angle > 185f && angle < 355f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Deposit"))
        {
            Debug.Log("Drill hit a deposit!");
            inDeposit = true;
            rb.velocity = Vector2.zero;
            targetDeposit = collision.gameObject;
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            rb.velocity = new Vector2(rb.velocity.x * -1, rb.velocity.y);
            transform.rotation = Quaternion.Euler(0, 0, 540 - aimAngle + 90);

        }

    }


    public void Mine()
    {
        if(targetDeposit != null)
            Destroy(targetDeposit);
            inDeposit = false;
    }
}
