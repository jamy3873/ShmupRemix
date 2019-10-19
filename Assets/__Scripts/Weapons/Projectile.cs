using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private BoundsCheck bndCheck;
    private Renderer rend;

    [Header("Set Dynamically")]
    public Rigidbody rigid;
    [SerializeField] //Forces following declaration to be settable in the inspector, even though it's private
    private WeaponType _type; //Undersore used for private variables, accessed by properties w/out the underscore
    private float birthTime;
    private float x0;

    public WeaponType type
    {
        get
        {
            return (_type);
        }
        set
        {
            SetType(value);
        }
    }

    void Awake()
    {
        birthTime = Time.time;
        x0 = transform.localPosition.x;
        bndCheck = GetComponent<BoundsCheck>();
        rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (tag == "ProjectileHero" && !bndCheck.onCamera(transform.position)) Destroy(gameObject);
        if (rigid.velocity.magnitude < 30 ) Destroy(gameObject);
    }

    public void SetType(WeaponType eType)
    {
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        rend.material.color = def.projectileColor;
    }

    public void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.gameObject;
        if (go.tag == "Walls" || go.tag == "Asteroid")
        {
            Destroy(gameObject);
        }
        if (go.layer == 14 && tag != "ProjectileEnemy") //Door layer
        {
            OpenDoor(go);
            Destroy(gameObject);
        }
    }

    private void OpenDoor(GameObject door)
    {
        Door d = door.GetComponent<Door>();
        WeaponType wt = d.weapon;
        if (_type == wt)
        {
            Destroy(door);
        }
    }
}
