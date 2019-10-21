using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelControl : MonoBehaviour
{
    public int levelIndex;
    public string levelName;
    public float rainbowTimeChange = 1.0f;
    public float rainbowTimeSince = 0f;
    public Renderer cubeRend;

    private void Start()
    {
        cubeRend = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (gameObject.name == "Exit") //Rainbow Effect
        {
            rainbowTimeSince += Time.deltaTime;
            if (rainbowTimeSince >= rainbowTimeChange)
            {
                cubeRend.material.color = new Color(
                    Random.value,
                    Random.value,
                    Random.value);
                rainbowTimeSince = 0f;
            }
            gameObject.transform.Rotate(Vector3.forward, (1800 * Time.deltaTime % 360));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        if (go.tag == "Hero")
        {
            SceneManager.LoadScene(levelName);
            
            //Restart
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
