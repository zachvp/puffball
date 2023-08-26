using System;
using UnityEngine;

public class DebugDraw : MonoBehaviour
{
    public Color color = Color.magenta;
    public float length = 1;

    public void Update()
    {
        DrawHorizontal();
        DrawVertical();
    }

    public void DrawHorizontal()
    {
        var start = transform.position;
        start.x -= length;

        var end = transform.position;
        end.x += length;

        Debug.DrawLine(start, end, color);
    }

    public void DrawVertical()
    {
        var start = transform.position;
        start.y -= length;

        var end = transform.position;
        end.y += length;

        Debug.DrawLine(start, end, color);
    }
}
