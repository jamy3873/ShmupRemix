using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCheck : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float radius = 1f;
    public bool keepOnScreen = true;

    [Header("Set Dynamically")]
    public bool isOnScreen;
    public bool offRight, offLeft, offUp, offDown;
    public float camWidth;
    public float camHeight;
    public Vector2 playArea;

    void Start()
    {
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;

        playArea = new Vector2(100, 100);
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        isOnScreen = true;
        offRight = offLeft = offUp = offDown = false;

        if (pos.x > playArea.x - radius)
        {
            pos.x = playArea.x - radius;
            offRight = true;
        }
        if (pos.x < -playArea.x + radius)
        {
            pos.x = -playArea.x + radius;
            offLeft = true;
        }
        if (pos.y > playArea.y - radius)
        {
            pos.y = playArea.y - radius;
            offUp = true;
        }
        if (pos.y < -playArea.y + radius)
        {
            pos.y = -playArea.y + radius;
            offDown = true;
        }

        isOnScreen = !(offRight || offLeft || offUp || offDown);
        if (keepOnScreen && !isOnScreen)
        {
            transform.position = pos;
            isOnScreen = true;
            offRight = offLeft = offUp = offDown = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Vector3 boundSize = new Vector3(playArea.x * 2, playArea.y * 2, 0.1f);
        Gizmos.DrawWireCube(Vector3.zero, boundSize);
    }
}
