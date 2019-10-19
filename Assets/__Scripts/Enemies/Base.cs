using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : Enemy
{
    [Header("Set in Inspector")]
    
    public WeaponType unlocksWeapon;
    private SpawnController spawner;
    

    void Start()
    {
        //Set Weapon Types
        foreach(Weapon w in weapons)
        {
            w.type = unlocksWeapon;
        }
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }

        spawner = GetComponent<SpawnController>();
        InvokeRepeating("spawnEnemy", .5f, 4f);
        InvokeRepeating("Shoot", .5f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (showingDamage && Time.time > damageDoneTime)
        {
            UnShowDamage();
        }
    }

    public override void Move()
    {
        transform.Rotate(Vector3.forward, (30 * Time.deltaTime % 360));
    }

    public void spawnEnemy()
    {
        if(!bndCheck.onCamera(transform.position) || (spawner.spawnAllowed && Main.S.enemyCount >= Main.S.maxEnemies))
        {
            spawner.spawnAllowed = false;
        }
        if(bndCheck.onCamera(transform.position) && Main.S.enemyCount < Main.S.maxEnemies)
        {
            spawner.spawnAllowed = true;
        }
        spawner.SpawnSomething();
    }

    public void Shoot()
    {
        if (bndCheck.onCamera(transform.position))
        {
            foreach (Weapon w in weapons)
            {
                w.Fire();
            }
        }
        
    }
}
