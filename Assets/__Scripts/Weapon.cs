using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    none,
    blaster,
    spread,
    phaser,
    missile,
    laser,
    shield
}

[System.Serializable] //Makes the following class serializable and editable in Unity inspector (for simple classes)
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    public string letter;
    public Color color = Color.white;
    public GameObject projectilePrefab;
    public Color projectileColor = Color.white;
    public float damageOnHit = 0;
    public float continuousDamage = 0;
    public float shotDelay = 0;
    public float velocity = 20;
}

public class Weapon : MonoBehaviour
{
    static public Transform PROJECTILE_ANCHOR;

    [Header("Set Dynamically")]
    [SerializeField]
    private WeaponType _type = WeaponType.none;
    public WeaponDefinition def;
    public GameObject collar;
    public float lastShotTime;
    private Renderer collarRend;

    private GameObject ship;

    void Start()
    {
        ship = transform.root.gameObject;
        collar = transform.Find("Collar").gameObject;
        collarRend = collar.GetComponent<Renderer>();
        
        //Call SetType() for default _type of WeaponType.none
        SetType(_type);

        //Dynamically create an anchor for all projectiles
        if(PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }
        //Find the fireDelegate of the root GameObject (Which is the hero)
        GameObject rootGO = transform.root.gameObject;
        if(rootGO.GetComponent<HeroShip>() != null)
        {
            rootGO.GetComponent<HeroShip>().fireDelegate += Fire;
        }
        else if (rootGO.GetComponent<HubShip>() != null)
        {
            rootGO.GetComponent<HubShip>().fireDelegate += Fire;
        }
    }

    public WeaponType type
    {
        get { return (_type); }
        set { SetType(value); }
    }

    public void SetType(WeaponType wt)
    {
        _type = wt; //Disable if WeaponType.none
        if (type == WeaponType.none)
        {
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            this.gameObject.SetActive(true);
        }
        //Dynamically set weapon's definition, color, and reset last shot time
        def = Main.GetWeaponDefinition(_type);
        collarRend.material.color = def.color;
        lastShotTime = 0;
    }
    
    public void Fire()
    {
        if (!gameObject.activeInHierarchy) return;
        if (Time.time - lastShotTime < def.shotDelay) return;

        Projectile p; //Create and send a projectile "upwards" and if firing downward, reverse y velocity
        Vector3 vel = ship.transform.up * def.velocity;

        switch (type)
        {
            case WeaponType.blaster:
                p = MakeProjectile();
                p.rigid.velocity = vel;
                p.transform.rotation = ship.transform.rotation;
                break;
            case WeaponType.spread:
                p = MakeProjectile();
                p.rigid.velocity = vel;
                p = MakeProjectile();
                p.transform.rotation = ship.transform.rotation;
                p.transform.rotation = Quaternion.AngleAxis(p.transform.rotation.z + 10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                p = MakeProjectile();
                p.transform.rotation = ship.transform.rotation;
                p.transform.rotation = Quaternion.AngleAxis(p.transform.rotation.z - 10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                p.transform.rotation = ship.transform.rotation;
                break;
        }
    }

    public Projectile MakeProjectile()
    {
        GameObject go = Instantiate<GameObject>(def.projectilePrefab);
        if(transform.parent.gameObject.tag == "Hero")
        {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        } else
        {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }
        go.transform.position = collar.transform.position;
        go.transform.SetParent(PROJECTILE_ANCHOR, true);
        Projectile p = go.GetComponent<Projectile>();
        p.type = type;
        lastShotTime = Time.time;
        return p;
    }
}
