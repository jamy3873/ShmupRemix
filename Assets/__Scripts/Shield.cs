using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float rotationsPerSecond = 0.1f;

    [Header("set Dynamically")]
    public int levelShown = 0;
    private int _currLevel = 0;
    private Hero parent;
    private Hero parentHero;

    Material mat;
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        parent = GetComponentInParent<Hero>();
        
    }

    void Update()
    {
        _currLevel = Mathf.FloorToInt(parent.shieldLevel);
        if (levelShown != _currLevel)
        {
            levelShown = _currLevel;
            mat.mainTextureOffset = new Vector2(0.2f * levelShown, 0);
        }
        float rZ = -(rotationsPerSecond * Time.time * 360) % 360f;
        transform.rotation = Quaternion.Euler(0, 0, rZ);
    }
}
