using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubShip : Hero
{
    public static HubShip S;
    [Header("Set in Inspector: HubShip")]
    public GameObject HeroShip;

    void Start()
    {
        S = this;
        rb = GetComponent<Rigidbody>();
        //ClearWeapons();
    }

    // Update is called once per frame
    void Update()
    {
        base.Move();
        Move();
        
        //Fire
        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate();
        }
        if (Time.time > invincibilityTimer)
        {
            Vulnerable();
        }
    }

    private void FixedUpdate()
    {

        rb.velocity = Vector3.zero;
    }

    public override void Move()
    {
        if (HeroShip != null)
        {
            transform.Rotate(Vector3.forward, (30 * Time.time % 360));
        }
        else
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 mouseOffset = new Vector3(Screen.width, Screen.height, 0);
            mousePos -= mouseOffset / 2;
            if (mousePos.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, -Vector3.Angle(mousePos.normalized, Vector3.up));
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, Vector3.Angle(mousePos.normalized, Vector3.up));
            }
        }
    }

}
