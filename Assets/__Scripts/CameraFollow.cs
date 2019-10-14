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
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }

}
