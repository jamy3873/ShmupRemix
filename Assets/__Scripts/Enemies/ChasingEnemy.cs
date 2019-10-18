using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingEnemy : Enemy {

    public Rigidbody rb;
	public GameObject target;
    public Vector3 directionToTarget;
	// Use this for initialization
	void Start () {
        if (Main.S.Hub != null){
            target = Main.S.Hub;
        }
        else if (Main.S.Hero != null)
        {
            target = Main.S.Hero;
        }
		
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		Move();

        if (showingDamage && Time.time > damageDoneTime)
        {
            UnShowDamage();
        }

        if (bndCheck != null && !bndCheck.isOnScreen)
        {
            Destroy(gameObject);
            Main.S.enemyCount--;
        }
        
        if (target == null)
        {
            CameraFollow cf = Camera.main.GetComponent<CameraFollow>();
            if (cf.target)
            {
                target = cf.target.gameObject;
            }
            
        }
    }

	public override void Move()
	{
		if (target != null) {
			directionToTarget = (target.transform.position - transform.position).normalized;
			rb.velocity = new Vector3 (directionToTarget.x * speed, directionToTarget.y * speed, 0);
		}
		else
			rb.velocity = Vector3.zero;
	}
}
