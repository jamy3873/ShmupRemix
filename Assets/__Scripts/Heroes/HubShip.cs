using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubShip : Hero
{
    public static HubShip S;
    [Header("Set in Inspector: HubShip")]
    public GameObject Hero;
    public float rotationSpeed;

    void Start()
    {
        S = this;
        Hero = Main.S.Hero;
        
        rb = GetComponent<Rigidbody>();
        ClearWeapons();
        weapons[0].type = WeaponType.blaster;

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
        if (Hero != null)
        {
            transform.Rotate(Vector3.forward, (rotationSpeed * Time.time % 360));
        }
        else
        {
            Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mousePos = new Vector3(mouse.x, mouse.y, 0) - transform.position;
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
