using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main S;
    static public Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

    [Header("Set in Inspector")]
    public GameObject Hub;
    public GameObject Hero;
    public GameObject[] prefabEnemies;
    public GameObject prefabAsteroid;
    public float        enemySpawnPerSecond = 0.5f;
    public float        enemyDefaultPadding = 0.1f;
    public int          maxEnemies = 15;
    public int          maxAsteroids = 10;
    public WeaponDefinition[] weaponDefinitions;
    public GameObject prefabPowerUp;
    public WeaponType[] powerUpFrequencey;

    private BoundsCheck bndCheck;
    public int enemyCount = 0;
    public int asteroidCount = 20;

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
            enemyCount--;
        }
    }

    public void AsteroidDestroyed(Asteroid a)
    {
        asteroidCount++;
        if (a.transform.localScale.x > 4)
        {
            Vector3 size = new Vector3(4, 4, 4);
            GameObject go1 = Instantiate<GameObject>(prefabAsteroid);
            go1.transform.localScale = size;
            go1.transform.position = a.transform.position;
            GameObject go2 = Instantiate<GameObject>(prefabAsteroid);
            go2.transform.localScale = size;
            go2.transform.position = a.transform.position;
        }
        else if (a.transform.localScale.x > 3)
        {
            Vector3 size = new Vector3(3, 3, 3);
            GameObject go1 = Instantiate<GameObject>(prefabAsteroid);
            go1.transform.localScale = size;
            go1.transform.position = a.transform.position;
            GameObject go2 = Instantiate<GameObject>(prefabAsteroid);
            go2.transform.localScale = size;
            go2.transform.position = a.transform.position;
        }
        else
        {
            if (Random.value <= a.powerUpDropChance)
            {
                int ndx = Random.Range(0, powerUpFrequencey.Length);
                WeaponType puType = powerUpFrequencey[ndx];
                
                GameObject go = Instantiate(prefabPowerUp) as GameObject;
                PowerUp pu = go.GetComponent<PowerUp>();
                pu.SetType(puType);
                pu.transform.position = a.transform.position;
            }
            asteroidCount--;
            SpawnAsteroids();
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

        //Spawn Asteroids
        SpawnAsteroids();
    }

    public void Update()
    {

    }

    public void SpawnAsteroids()
    {
        for(int i = asteroidCount; i < maxAsteroids; i++)
        {
            GameObject go = Instantiate<GameObject>(prefabAsteroid);

            float enemyPadding = enemyDefaultPadding;
            if (go.GetComponent<BoundsCheck>() != null)
            {
                enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
            }

            Vector3 pos = Vector3.zero;
            float xMin = -bndCheck.playArea.x + enemyPadding;
            float xMax = bndCheck.playArea.x - enemyPadding;
            float yMin = -bndCheck.playArea.y + enemyPadding;
            float yMax = bndCheck.playArea.y - enemyPadding;
            pos.x = Random.Range(xMin, xMax);
            pos.y = Random.Range(yMin, yMax);
            //Ensures wont spawn on player

            float distFromShip = 60;
            GameObject ship = Hub;
            if(ship != null)
            {
                ship = Hero;
            }
            while(distFromShip < 60)
            {
                pos.x = Random.Range(xMin, xMax);
                pos.y = Random.Range(yMin, yMax);
                distFromShip = Vector3.Distance(ship.transform.position, pos);
            }

            go.transform.position = pos;
            asteroidCount++;
        }
    }

    public void SpawnEnemy()
    {
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

        float enemyPadding = enemyDefaultPadding;
        if (go.GetComponent<BoundsCheck>() != null)
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        Vector3 pos = Vector3.zero;
        float xMin = -bndCheck.playArea.x + enemyPadding;
        float xMax = bndCheck.playArea.x - enemyPadding;
        float yMin = -bndCheck.playArea.y + enemyPadding;
        float yMax = bndCheck.playArea.y - enemyPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = Random.Range(yMin, yMax);

        //Ensures spawning off camera
        while (bndCheck.onCamera(pos))
        {
            pos.x = Random.Range(xMin, xMax);
            pos.y = Random.Range(yMin, yMax);
        }
            
        go.transform.position = pos;

        enemyCount++;

        if (enemyCount < maxEnemies)
        {
            Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
        }
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
