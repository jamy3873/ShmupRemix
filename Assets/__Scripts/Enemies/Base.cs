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
        spawner = GetComponent<SpawnController>();
        InvokeRepeating("spawnEnemy", .5f, 4f);
    }

    // Update is called once per frame
    void Update()
    {
        if (showingDamage && Time.time > damageDoneTime)
        {
            UnShowDamage();
        }
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
}
