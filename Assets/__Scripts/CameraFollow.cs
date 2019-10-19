using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    static public CameraFollow S;

    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    private void Start()
    {
        if (S == null)
        {
            S = this;
        }
        ResetTarget();
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
        else
        {
            ResetTarget();
        }
    }

    public void ResetTarget()
    {
        if (target == null)
        {
            if (HubShip.S)
            {
                target = HubShip.S.transform;
            }
            else if (HeroShip.S)
            {
                target = HeroShip.S.transform;
            }
                
        }
    }
}
