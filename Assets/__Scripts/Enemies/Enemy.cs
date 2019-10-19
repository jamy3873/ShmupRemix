using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    static public Transform ENEMY_ANCHOR;

    [Header("Set in Inspector")]
    public float    speed = 10f;
    public float    fireRate = 0.3f;
    public float    health = 10;
    public int      score = 100;
    public float    showDamageDuration = 0.1f;
    public float    powerUpDropChance = 1f;
    public Color baseColor;
    public Weapon[] weapons;

    [Header("Set Dynamically: Enemy")]
    public Color[]      originalColors;
    public Material[]   materials;
    public bool         showingDamage = false;
    public float        damageDoneTime;
    public bool         notifiedOfDestruction = false;


    protected BoundsCheck bndCheck;
    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        //Get mats and colors for this enemy and its children
        if (GetComponent<Renderer>())
        {
            GetComponent<Renderer>().material.color = baseColor;
        }
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for (int i = 0; i<materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }

        if (ENEMY_ANCHOR == null)
        {
            GameObject go = new GameObject("_EnemyAnchor");
            go.tag = "Enemy";
            ENEMY_ANCHOR = go.transform;
        }
        //transform.SetParent(ENEMY_ANCHOR, true);
    }

    public Vector3 pos
    {
        get
        {
            return this.transform.position;
        }
        set
        {
            this.transform.position = value;
        }
    }

    void Update()
    {
        Move();
        if(showingDamage && Time.time > damageDoneTime)
        {
            UnShowDamage();
        }

        if (bndCheck != null && !bndCheck.isOnScreen)
        {
            Destroy(gameObject);
            Main.S.enemyCount--;
        }
    }

    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    public virtual void OnCollisionEnter(Collision collision)
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
                        Main.S.ShipDestroyed(this);
                    }
                    notifiedOfDestruction = true;
                    Destroy(this.gameObject);
                }
                Destroy(otherGO);
                break;
        }
    }

    protected void ShowDamage()
    {
        foreach(Material m in materials)
        {
            m.color = Color.red;
        }
        showingDamage = true;
        damageDoneTime = Time.time + showDamageDuration;
    }

    protected void UnShowDamage()
    {
        for(int i = 0; i < materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
        showingDamage = false;
    }
}
