using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject poi;
    public GameObject[] panels;
    public float scrollSpeed = -30f;
    public float motionMult = 0.25f;

    private BoundsCheck CameraBounds;

    private float panelHt;
    private float depth;
    void Start()
    {
        CameraBounds = Camera.main.GetComponent<BoundsCheck>();
        if (Main.S.Hub != null) {
            poi = Main.S.Hub;
        }
        else if (Main.S.Hero != null)
        {
            poi = Main.S.Hero;
        }
        else
        {
            Debug.Log("Failed to attach ship to parallax background!");
        }

        panelHt = panels[0].transform.localScale.y;
        depth = 20;

        panels[0].transform.position = new Vector3(0, 0, depth);
    }

    // Update is called once per frame
    void Update()
    {
        float tY = 0, tX = 0;

        if (poi != null)
        {
            tX = -poi.transform.position.x * motionMult;
            tY = -poi.transform.position.y * motionMult;
            transform.position = new Vector3(poi.transform.position.x, poi.transform.position.y, 21);
        }

        //Position panels[0]
        panels[0].transform.position = new Vector3(tX, tY, depth);
    }
}
