 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {

    public float spawnDelay = 5f;
	public Transform[] spawnPoints;
	public GameObject[] enemies;
	int randomSpawnPoint, randomEnemy;
    public float spawnFrequency = 3f;
	public bool spawnAllowed;
    public bool spawnRepeating;


	// Use this for initialization
	void Start ()
    {
		if (spawnRepeating)
        {
            InvokeRepeating("SpawnSomething", 0f, spawnFrequency);
        }
	}

    private void Update()
    {

    }

    public void SpawnSomething()
	{
		if (spawnAllowed) {
			randomSpawnPoint = Random.Range (0, spawnPoints.Length);
			randomEnemy = Random.Range (0, enemies.Length);
			//GameObject e = 
                Instantiate (enemies[randomEnemy], spawnPoints[randomSpawnPoint].position, Quaternion.identity);
		}
	}

}
