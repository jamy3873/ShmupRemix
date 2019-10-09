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

    private float panelHt;
    private float depth;
    void Start()
    {
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
        panels[1].transform.position = new Vector3(0, panelHt, depth);
    }

    // Update is called once per frame
    void Update()
    {
        float tY, tX = 0;
        tY = Time.time * scrollSpeed % panelHt + (panelHt * 0.5f);

        if (poi != null)
        {
            tX = -poi.transform.position.x * motionMult;
        }

        //Position panels[0]
        panels[0].transform.position = new Vector3(tX, 0, depth);
        //Position panels[1] to make a continuous starfield
        panels[1].transform.position = new Vector3(tX, 0, depth);

    }
}
