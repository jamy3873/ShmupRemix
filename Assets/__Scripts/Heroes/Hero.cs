using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    public float gameRestartDelay = 2f;
    public float invincibilityTimer = 5f;
    public Weapon[] weapons;

    [Header("Set Dynamically")]
    [SerializeField]
    protected Rigidbody rb;
    protected float _shieldLevel = 1;

    protected GameObject lastTriggerGo = null;
    protected GameObject lastCollGo = null;

    public delegate void WeaponFireDelegate();
    public WeaponFireDelegate fireDelegate;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public virtual void Move()
    {
        //Move
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        if (go == lastTriggerGo) return;
        lastTriggerGo = go;

        if (go.tag == "Enemy" || other.tag == "Asteroid" || other.tag == "ProjectileEnemy")
        {
            shieldLevel--;
            if (other.tag == "ProjectileEnemy")
            {
                Destroy(go);
            }
        }
        else if (go.tag == "PowerUp")
        {
            AbsorbPowerUp(go);
        }
        else
        {
            print("Hero triggered by non-Enemy: " + go.name);
        }
    }


    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();
        switch (pu.type)
        {
            case WeaponType.shield:
                shieldLevel++;
                break;
            default:
                if (pu.type == weapons[0].type)
                {
                    Weapon w = GetEmptyWeaponSlot();
                    if (w != null){
                        w.SetType(pu.type);
                    }
                }
                else
                {
                    ClearWeapons();
                    weapons[0].SetType(pu.type);
                }
                break;
        }
        pu.AbsorbedBy(this.gameObject);
    }

    public float shieldLevel
    {
        get
        {
            return (_shieldLevel);
        }
        set
        {
            _shieldLevel = Mathf.Min(value, 4);
            if (value < 0)
            {
                if (gameObject == Main.S.Hub && Main.S.Hero != null)
                {
                    CameraFollow cf = Camera.main.GetComponent<CameraFollow>();
                    cf.target = Main.S.Hero.transform;
                }
                Destroy(gameObject);
            }
        }
    }

    Weapon GetEmptyWeaponSlot()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if(weapons[i].type == WeaponType.none)
            {
                return weapons[i];
            }
        }
        return null;
    }

    protected void ClearWeapons()
    {
        foreach(Weapon w in weapons)
        {
            w.SetType(WeaponType.none);
        }
    }
}
