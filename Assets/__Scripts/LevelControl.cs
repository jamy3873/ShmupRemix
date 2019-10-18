using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelControl : MonoBehaviour
{
    public int levelIndex;
    public string levelName;

    private void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        print(go);
        if (go.tag == "Hero")
        {
            SceneManager.LoadScene(levelName);

            //Restart
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
