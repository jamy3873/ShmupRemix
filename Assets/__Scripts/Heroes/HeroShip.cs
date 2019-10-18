using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroShip : Hero
{
    public static HeroShip S;
    public GameObject Hub;

    public int distFromHub = 30;

    void Start()
    {
        S = this;
        ClearWeapons();
        weapons[0].SetType(WeaponType.blaster);

        rb = GetComponent<Rigidbody>();
        if (Hub)
        {
            transform.position = Hub.transform.position + transform.right * distFromHub;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        //Fire
        if (Input.GetAxis("Fire1") == 1 && fireDelegate != null)
        {
            fireDelegate();
        }
        if (Time.time > invincibilityTimer)
        {
            Vulnerable();
        }
    }

    public override void Move()
    {
        if (Hub != null)
        {


            /*Vector3 mousePos = Input.mousePosition;
            Vector3 mouseOffset = new Vector3(Screen.width, Screen.height, 0);
            mousePos -= mouseOffset / 2;*/
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Hub.transform.position;

            transform.position = Hub.transform.position + (mousePos.normalized * distFromHub);
            if (mousePos.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, -Vector3.Angle(mousePos.normalized, Vector3.up));
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, Vector3.Angle(mousePos.normalized, Vector3.up));
            }

        }
        else
        {
            rb.velocity = Vector3.zero;
            float xAxis = Input.GetAxis("Horizontal");
            float yAxis = Input.GetAxis("Vertical");

            Vector3 pos = transform.position;
            pos.x += xAxis * speed * Time.deltaTime;
            pos.y += yAxis * speed * Time.deltaTime;
            transform.position = pos;

            Vector3 mousePos = Input.mousePosition;
            Vector3 mouseOffset = new Vector3(Screen.width, Screen.height, 0);
            mousePos -= mouseOffset / 2;
            Quaternion rot = transform.rotation;
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
