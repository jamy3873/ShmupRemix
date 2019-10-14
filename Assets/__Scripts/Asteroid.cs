using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : Enemy
{
    static public Transform ASTEROID_ANCHOR;

    private Rigidbody rigid;
    private float maxSpeed = 20;
    public Vector3 rotPerSecond;
    public Vector2 rotMinMax = new Vector2(15, 90);
    void Start()
    {
        if (ASTEROID_ANCHOR == null)
        {
            GameObject go = new GameObject("_AsteroidAnchor");
            go.tag = "Asteroid";
            ASTEROID_ANCHOR = go.transform;
        }
        transform.SetParent(ASTEROID_ANCHOR, true);

        //Random position, velocity, and rotation
        rigid = GetComponent<Rigidbody>();
        Vector3 vel = Random.onUnitSphere;
        vel.z = 0;
        rigid.velocity = vel.normalized * Random.Range(3, maxSpeed);
        Main.S.asteroidCount++;

        //Set random rotation
        transform.rotation = Quaternion.identity;
        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y),
            Random.Range(rotMinMax.x, rotMinMax.y),
            Random.Range(rotMinMax.x, rotMinMax.y));
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);
        if (showingDamage && Time.time > damageDoneTime)
        {
            UnShowDamage();
        }
        if (bndCheck != null && !bndCheck.isOnScreen)
        {
            Destroy(gameObject);
            Main.S.asteroidCount--;
        }
    }

    public override void Move()
    {
        
    }

    public override void OnCollisionEnter(Collision collision)
    {
        GameObject otherGO = collision.gameObject;
        switch (otherGO.tag)
        {
            case "ProjectileHero":
                Projectile p = otherGO.GetComponent<Projectile>();
                if (!bndCheck.isOnScreen)
                {
                    Destroy(otherGO);
                    break;
                }

                ShowDamage();
                health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                if (health <= 0)
                {
                    if (!notifiedOfDestruction)
                    {
                        Main.S.AsteroidDestroyed(this);
                        
                    }
                    notifiedOfDestruction = true;
                    Destroy(this.gameObject);
                }
                Destroy(otherGO);
                break;
        }
    }
}
