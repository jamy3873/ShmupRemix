﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main S;
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;
    public float        enemySpawnPerSecond = 0.5f;
    public float        enemyDefaultPadding = 0.1f;
    public int          maxEnemies = 20;
    public WeaponDefinition[] weaponDefinitions;
    public GameObject prefabPowerUp;
    public WeaponType[] powerUpFrequencey = new WeaponType[] {
                            WeaponType.blaster, WeaponType.spread,
                            WeaponType.blaster, WeaponType.shield};

    private BoundsCheck bndCheck;
    public int enemyCount = 0;

    public void ShipDestroyed(Enemy e)
    {
        if (Random.value <= e.powerUpDropChance)
        {
            int ndx = Random.Range(0, powerUpFrequencey.Length);
            WeaponType puType = powerUpFrequencey[ndx];

            GameObject go = Instantiate(prefabPowerUp) as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();
            pu.SetType(puType);
            pu.transform.position = e.transform.position;
        }
    }

    void Awake()
    {
        S = this;
        bndCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);

        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach(WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }
    }

    public void Update()
    {

    }

    public void SpawnEnemy()
    {
        if (enemyCount < maxEnemies)
        {
            int ndx = Random.Range(0, prefabEnemies.Length);
            GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

            float enemyPadding = enemyDefaultPadding;
            if (go.GetComponent<BoundsCheck>() != null)
            {
                enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
            }

            Vector3 pos = Vector3.zero;
            float xMin = -bndCheck.camWidth + enemyPadding;
            float xMax = bndCheck.camWidth - enemyPadding;
            float yMin = -bndCheck.camHeight + enemyPadding;
            float yMax = bndCheck.camHeight - enemyPadding;
            pos.x = Random.Range(xMin, xMax);
            pos.y = Random.Range(yMin, yMax);
            go.transform.position = pos;

            enemyCount++;
        }
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }

    public void DelayedRestart(float delay)
    {
        Invoke("Restart", delay);
    }

    public void Restart()
    {
        SceneManager.LoadScene("_Scene_0");
    }

    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        if (WEAP_DICT.ContainsKey(wt))
        {
            return (WEAP_DICT[wt]);
        }
        return (new WeaponDefinition());
    }
}
