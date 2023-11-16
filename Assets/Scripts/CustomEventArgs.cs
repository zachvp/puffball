using UnityEngine;
using System;

[Serializable]
public struct PCInputArgs
{
    public CoreActionMap.Player.Action type;

    public Vector2 handMove;
    public float move;
    public bool handAction;
    public bool grip;
    public bool jump;
    public bool start;

    public override string ToString()
    {
        return JsonUtility.ToJson(this);
    }
}
