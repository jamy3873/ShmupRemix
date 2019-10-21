using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAndShoot : ChasingEnemy
{

    private void Start()
    {
        if (Main.S.Hub != null)
        {
            target = Main.S.Hub;
        }
        else if (Main.S.Hero != null)
        {
            target = Main.S.Hero;
        }
        rb = GetComponent<Rigidbody>();

        InvokeRepeating("shoot", 0f, fireRate);
    }
    
    void Update()
    {
        Move();

        if (showingDamage && Time.time > damageDoneTime)
        {
            UnShowDamage();
        }

        if (bndCheck != null && !bndCheck.isOnScreen)
        {
            Destroy(gameObject);
            Main.S.enemyCount--;
        }

        if (target == null)
        {
            CameraFollow cf = Camera.main.GetComponent<CameraFollow>();
            if (cf.target != null) target = cf.target.gameObject;
        }
    }

    public override void Move()
    {
        if (target != null)
        {
            directionToTarget = (target.transform.position - transform.position).normalized;
            transform.up = directionToTarget;
            rb.velocity = new Vector3(directionToTarget.x * speed, directionToTarget.y * speed, 0);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
            
    }

    public void shoot()
    {
        if (bndCheck.onCamera(transform.position))
        {
            for (int w = 0; w < weapons.Length; w++)
            {
                weapons[w].Fire();
            }
        }
        
    }
}
