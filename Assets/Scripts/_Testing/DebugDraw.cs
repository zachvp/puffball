using System;
using UnityEngine;

public class DebugDraw : MonoBehaviour
{
    public Color color = Color.magenta;
    public float length = 1;
    public Style style;

    public void Update()
    {
        switch (style)
        {
            case Style.CROSS:
                DrawHorizontal();
                DrawVertical();
                break;
            case Style.CIRCLE:
                
                break;
            default:
                Debug.LogError($"unhandled style: {style}");
                break;
        }
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

    public enum Style
    {
        CROSS,
        CIRCLE
    }
}
