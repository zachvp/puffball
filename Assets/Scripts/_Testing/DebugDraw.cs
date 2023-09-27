using System;
using UnityEngine;

public class DebugDraw : MonoBehaviour
{
    public Color color = Color.magenta;
    public float size = 1;
    public Style style;
    public Vector3 offset;

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
            Gizmos.DrawWireSphere(transform.position + offset, size);
        }
    }

    public void DrawHorizontal()
    {
        var start = transform.position + offset;
        start.x -= size;

        var end = transform.position + offset;
        end.x += size;

        Debug.DrawLine(start, end, color);
    }

    public void DrawVertical()
    {
        var start = transform.position + offset;
        start.y -= size;

        var end = transform.position + offset;
        end.y += size;

        Debug.DrawLine(start, end, color);
    }

    public enum Style
    {
        CROSS,
        FILL
    }
}
