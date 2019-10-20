using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Part
{
    [Header("Set in Inspector")]
    public string name;
    public float health;
    public string[] protectedBy;

    [HideInInspector]
    public GameObject go;
    [HideInInspector]
    public Material mat;
}

public class Enemy_4 : Enemy
{
    private Vector3 p0, p1;
    private float timeStart;
    private float moveDuration = 3f;
    public Part[] parts;
    public GameObject cockpit;

    void Start()
    {
        //cockpit = transform.Find("Cockpit").gameObject;

        p0 = p1 = pos;
        InitMovement();

        Transform t;
        foreach (Part prt in parts)
        {
            t = transform.Find(prt.name);
            if (t != null)
            {
                prt.go = t.gameObject;
                prt.mat = prt.go.GetComponent<Renderer>().material;
            }
        }
        InvokeRepeating("Shoot1", 3f, 1f);
        InvokeRepeating("Shoot2", 6f, 1.2f);
        InvokeRepeating("Shoot3", 8f, .8f);
        InvokeRepeating("Shoot4", 10f, 4f);
    }

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.forward, (-10 * Time.deltaTime % 360));
        weapons[6].transform.Rotate(Vector3.forward, (25 * Time.deltaTime % 360));
        weapons[7].transform.Rotate(Vector3.forward, (-25 * Time.deltaTime % 360));
    }

    void InitMovement()
    {
        /*p0 = p1;
        float widMinRad = 150;
        float hgtMinRad = 100;
        p1.x = Random.Range(-widMinRad, widMinRad);
        p1.y = Random.Range(-hgtMinRad, hgtMinRad)+120;*/
        p0 = p1;
        if (CameraFollow.S && CameraFollow.S.target)
        {
            p1 = CameraFollow.S.target.position;
        }
        

        timeStart = Time.time;
    }

    public override void Move()
    {
        float u = (Time.time - timeStart) / moveDuration;

        if (u >= 1)
        {
            InitMovement();
            u = 0;
        }

        u = 1 - Mathf.Pow(1 - u, 2);
        pos = (1 - u) * p0 + u * p1;
    }

    Part FindPart(string n)
    {
        foreach (Part prt in parts)
        {
            if (prt.name == n) return prt;
        }
        return null;
    }

    Part FindPart(GameObject go)
    {
        foreach (Part prt in parts)
        {
            if (prt.go == go) return prt;
        }
        return null;
    }

    bool Destroyed(GameObject go)
    {
        return Destroyed(FindPart(go));
    }

    bool Destroyed(string n)
    {
        return Destroyed(FindPart(n));
    }

    bool Destroyed(Part prt)
    {
        if (prt == null) return true;
        return (prt.health <= 0);
    }

    void ShowLocalizedDamage(Material m)
    {
        m.color = Color.red;
        damageDoneTime = Time.time + showDamageDuration;
        showingDamage = true;
    }

    public override void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        switch (other.tag)
        {
            case "ProjectileHero":
                Projectile p = other.GetComponent<Projectile>();
                //Hurt Enemy
                GameObject goHit = collision.contacts[0].thisCollider.gameObject;
                Part prtHit = FindPart(goHit);
                if (prtHit == null)
                {
                    goHit = collision.contacts[0].otherCollider.gameObject;
                    prtHit = FindPart(goHit);
                }
                //Check if part is protected
                if (prtHit.protectedBy != null)
                {
                    foreach (string s in prtHit.protectedBy)
                    {
                        if (!Destroyed(s))
                        {
                            Destroy(other);
                            return;
                        }
                    }
                }
                //If not, make it take damage
                prtHit.health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                ShowLocalizedDamage(prtHit.mat);
                if (prtHit.health <= 0)
                {
                    prtHit.go.SetActive(false);
                    ScoreKeeper.S.UpdateScore(25);
                }
                //Check if whole ship is destroyed
                bool allDestroyed = true;
                foreach(Part prt in parts)
                {
                    if (!Destroyed(prt))
                    {
                        allDestroyed = false;
                        break;
                    }
                }
                if (allDestroyed)
                {
                    Main.S.ShipDestroyed(this);
                    Destroy(this.gameObject);
                }
                Destroy(other);
                break;

        }
    }

    public void Shoot1()
    {
        weapons[0].Fire();
        weapons[1].Fire();
        
    }

    public void Shoot2()
    {
        weapons[2].Fire();
        weapons[3].Fire();
    }
    public void Shoot3()
    {
        weapons[4].Fire();
        weapons[5].Fire();
        weapons[6].Fire();
        weapons[7].Fire();
    }

    public void Shoot4()
    {
        weapons[8].Fire();
        weapons[9].Fire();
        weapons[10].Fire();
        weapons[11].Fire();
    }
}
