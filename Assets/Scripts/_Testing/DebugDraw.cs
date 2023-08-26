using System;
using UnityEngine;

public class DebugDraw : MonoBehaviour
{
    public Color color = Color.magenta;
    public float length = 1;

    public void Update()
    {
        var start = transform.position;
        start.x -= length;

        var end = transform.position;
        end.x += length;

        Debug.DrawLine(start, end, color);

        start.y -= length;
        end.y += length;

        start.x = transform.position.x;
        end.x = transform.position.x;
        Debug.DrawLine(start, end, color);
    }
}
