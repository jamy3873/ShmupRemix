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
        transform.position = transform.right * distFromHub;
        transform.rotation = Quaternion.Euler(0, 0, -90);
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
            Vector3 targetDir = mousePos - Hub.transform.position;
            transform.position = Hub.transform.position + (targetDir.normalized * distFromHub);

            transform.LookAt(Hub.transform.position + targetDir.normalized,-Vector3.right);
        }
        else
        {
            base.Move();
        }
    }


}
