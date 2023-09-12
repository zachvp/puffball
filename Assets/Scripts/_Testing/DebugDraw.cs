using System;
using UnityEngine;

public class DebugDraw : MonoBehaviour
{
    public Color color = Color.magenta;
    public float size = 1;
    public Style style;

    public void Update()
    {
        switch (style)
        {
            case Style.CROSS:
                DrawHorizontal();
                DrawVertical();
                break;
        }
    }

    public void OnDrawGizmos()
    {
        if (enabled && style == Style.FILL)
        {
            Gizmos.color = color;
            Gizmos.DrawWireSphere(transform.position, size);
        }
    }

    public void DrawHorizontal()
    {
        var start = transform.position;
        start.x -= size;

        var end = transform.position;
        end.x += size;

        Debug.DrawLine(start, end, color);
    }

    public void DrawVertical()
    {
        var start = transform.position;
        start.y -= size;

        var end = transform.position;
        end.y += size;

        Debug.DrawLine(start, end, color);
    }

    public enum Style
    {
        CROSS,
        FILL
    }
}
