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
    public float invincibilityTimer;
    public float invincibleDuration;
    public bool invincible = false;
    public Weapon[] weapons;

    [Header("Prefabs for Respawns")]
    public GameObject heroPrefab;
    public GameObject hubPrefab;

    [Header("Set Dynamically")]
    [SerializeField]
    protected Material[] materials;
    protected Color[] originalColors;
    protected Rigidbody rb;
    protected float _shieldLevel = 1;

    protected GameObject lastTriggerGo = null;
    protected GameObject lastCollGo = null;

    public delegate void WeaponFireDelegate();
    public WeaponFireDelegate fireDelegate;

    void Awake()
    {
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
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
        if (invincible) return;
        lastTriggerGo = go;

        if (go.tag == "Enemy" || other.tag == "Asteroid" || other.tag == "ProjectileEnemy")
        {
            shieldLevel--;
            if (other.tag == "ProjectileEnemy")
            {
                Destroy(go);
            }
            Invulnerable();
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
            case WeaponType.newLife:
                ReviveShip();
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
                //Reset Camera Target
                if (gameObject == Main.S.Hub && Main.S.Hero != null)
                {
                    CameraFollow cf = Camera.main.GetComponent<CameraFollow>();
                    cf.target = Main.S.Hero.transform;
                }
                Destroy(gameObject);
                Main.S.powerUpFrequencey.Add(WeaponType.newLife);
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

    protected void Invulnerable()
    {
        foreach (Material m in materials)
        {
            m.color = Color.cyan;
        }
        invincibilityTimer = Time.time + invincibleDuration;
        invincible = true;
    }

    protected void Vulnerable()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = Color.white;
        }
        invincible = false;
    }

    private GameObject ReviveShip()
    {
        if (Main.S.Hero == null)
        {
            GameObject newShip =  Instantiate<GameObject>(heroPrefab);
            newShip.GetComponent<HeroShip>().Hub = HubShip.S.gameObject;
            HubShip.S.HeroShip = newShip;
            Main.S.Hero = newShip;
            return newShip;
        }
        if (Main.S.Hub == null)
        {
            GameObject newShip = Instantiate<GameObject>(hubPrefab);
            newShip.GetComponent<HubShip>().HeroShip = HeroShip.S.gameObject;

            //Set position and camera
            Vector3 pos = transform.position;
            newShip.transform.position = new Vector3(pos.x, pos.y, pos.z);

            CameraFollow.S.target = newShip.transform;
            HeroShip.S.Hub = newShip;
            Main.S.Hub = newShip;
            return newShip;
        }
        Main.S.powerUpFrequencey.Remove(WeaponType.newLife);
        return null;
    }
}
