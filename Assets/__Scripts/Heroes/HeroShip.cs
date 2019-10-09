using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroShip : Hero
{
    public GameObject Hub;

    public int distFromHub = 30;

    void Start()
    {
        ClearWeapons();
        weapons[0].SetType(WeaponType.blaster);
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
    }

    public override void Move()
    {
        if (Hub != null)
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 mouseOffset = new Vector3(Screen.width, Screen.height, 0);
            mousePos -= mouseOffset/2;

            Vector3 targetDir = mousePos - Hub.transform.up;
            transform.position = Hub.transform.position + (targetDir.normalized * distFromHub);

            Quaternion rot = transform.rotation;
            if(targetDir.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, -Vector3.Angle(targetDir.normalized, Hub.transform.up));
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, Vector3.Angle(targetDir.normalized, Hub.transform.up));
            }

        }
        else
        {
            base.Move();
        }


    }

    
}
