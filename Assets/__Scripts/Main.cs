using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main S;
    static public Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

    [Header("Set in Inspector")]

    public Transform PLAYERS;
    public Transform ENEMIES;
    public GameObject hubPrefab;
    public GameObject heroPrefab;
    public GameObject[] prefabEnemies;
    public GameObject prefabAsteroid;
    public GameObject bossPrefab;
    public float        enemySpawnPerSecond = 0.5f;
    public float        enemyDefaultPadding = 0.1f;
    public int          maxEnemies = 15;
    public int          maxAsteroids = 10;
    public WeaponDefinition[] weaponDefinitions;
    public GameObject prefabPowerUp;
    public List<WeaponType> powerUpFrequencey;

    private BoundsCheck bndCheck;
    private SpawnController Spawner;
    public int enemyCount = 0;
    public int asteroidCount = 20;

    [Header("Set Dynamically")]
    public GameObject Hub;
    public GameObject Hero;
    private bool wonGame = false;

    public void ShipDestroyed(Enemy e)
    {
        Base b = e.GetComponent<Base>();
        if (b)
        {
            powerUpFrequencey.Add(b.unlocksWeapon);
            WeaponType puType = b.unlocksWeapon;
            GameObject go = Instantiate(prefabPowerUp) as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();
            pu.SetType(puType);
            pu.transform.position = e.transform.position;
        }
        else if (e.gameObject.name == "Enemy_4")
        {
            ScoreKeeper.S.UpdateScore("YOU WIN!");
            wonGame = true;
            DelayedRestart(5);
            /*GameObject boss = Instantiate<GameObject>(bossPrefab,PLAYERS,true);
            boss.transform.position = new Vector3(0, 200, 0);*/
        }
        else if (Random.value <= e.powerUpDropChance)
        {
            int ndx = Random.Range(0, powerUpFrequencey.Count);
            WeaponType puType = powerUpFrequencey[ndx];
            
            GameObject go = Instantiate(prefabPowerUp) as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();
            pu.SetType(puType);
            pu.transform.position = e.transform.position;
            enemyCount--;
        }
        ScoreKeeper.S.UpdateScore(e.score);
    }

    public void AsteroidDestroyed(Asteroid a)
    {
        asteroidCount--;
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
                int ndx = Random.Range(0, powerUpFrequencey.Count);
                WeaponType puType = powerUpFrequencey[ndx];
                
                GameObject go = Instantiate(prefabPowerUp) as GameObject;
                PowerUp pu = go.GetComponent<PowerUp>();
                pu.SetType(puType);
                pu.transform.position = a.transform.position;
            }
            SpawnAsteroids();
        }
        ScoreKeeper.S.UpdateScore(a.score);
    }

    void Awake()
    {
        S = this;
        bndCheck = GetComponent<BoundsCheck>();
        Spawner = GetComponent<SpawnController>();

        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach (WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }

    }

    private void Start()
    {
        if (Hub == null)
        {
            if (HubShip.S)
            {
                Hub = HubShip.S.gameObject;
                
            }
            else if (transform.root.gameObject.scene.buildIndex == 1)
            {
                Transform spawn = transform;
                Vector3 pos = spawn.position;
                spawn.position = new Vector3(pos.x, pos.y, 0);
                Hub = Instantiate<GameObject>(hubPrefab, spawn.position, Quaternion.identity);
                Hub.transform.SetParent(PLAYERS, true);
                HubShip hubScript = Hub.GetComponent<HubShip>();
                hubScript.ResetWeapons();
                for (int i = 0; i < hubScript.weapons.Length; i++)
                {
                    hubScript.weapons[i].type = WeaponType.blaster;
                }

            }
        }
        if (Hero == null)
        {
            if (HeroShip.S)
            {
                Hero = HeroShip.S.gameObject;
            }
            else if (transform.root.gameObject.scene.buildIndex == 1)
            {
                Transform spawn = transform;
                Vector3 pos = spawn.position;
                spawn.position = new Vector3(pos.x, pos.y, 0);
                Hero = Instantiate<GameObject>(heroPrefab, spawn.position, Quaternion.identity);
                Hero.transform.SetParent(PLAYERS, true);
                HeroShip heroScript = Hero.GetComponent<HeroShip>();
                heroScript.ResetWeapons();
                heroScript.weapons[0].type = WeaponType.spread;

                /*for (int i = 0; i < heroScript.weapons.Length; i++)
                {
                    heroScript.weapons[i].type = WeaponType.blaster;
                }*/
            }
        }

        //Spawn Asteroids
        for (int point = 0; point < Spawner.spawnPoints.Length; point++)
        {
            Instantiate(prefabAsteroid, Spawner.spawnPoints[point].position, Quaternion.identity);
        }
        //Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }

    public void Update()
    {
        if(Hub == null && Hero == null)
        {
            DelayedRestart(2);
        }
    }

    public void SpawnAsteroids()
    {
        if (asteroidCount < maxAsteroids)
        {
            Spawner.SpawnSomething();
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
        if (wonGame)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
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
