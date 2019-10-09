using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public WeaponType weapon;
    public Color color;

    private Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        mat.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
