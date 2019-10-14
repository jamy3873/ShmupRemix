using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : Enemy
{
    [Header("Set in Inspector")]


    private SpawnController spawner;
    private BoundsCheck bndCheck;
    private Vector3 spawnOffset = new Vector3(0, 10, 0);



    void Start()
    {
        spawner = GetComponent<SpawnController>();
        bndCheck = GetComponent<BoundsCheck>();
        InvokeRepeating("spawnEnemy", 1f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (showingDamage && Time.time > damageDoneTime)
        {
            UnShowDamage();
        }
    }

    public override void Move()
    {

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
