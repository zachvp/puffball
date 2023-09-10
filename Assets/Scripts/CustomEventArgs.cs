using UnityEngine;
using System;
using UnityEngine.InputSystem;

public struct InputButtonArgs
{
    public int playerID;
    public InputActionPhase phase;
    public CoreActionMap.Player.Action action;
}

[Serializable]
public struct InputAxis1DArgs
{
    public int playerID;
    public CoreActionMap.Player.Action action;
    public int axis;
}

[Serializable]
public struct InputAxis2DArgs
{
    public int playerID;
    public CoreActionMap.Player.Action action;
    public Vector2 axis;
}

[Serializable]
public struct PCInputArgs
{
    public CoreActionMap.Player.Action type;
    public int playerIndex;

    public Vector2 handMove;
    // todo: define others and refactor
    public float vFloat;
    public bool vBool;

    

    public override string ToString()
    {
        return JsonUtility.ToJson(this);
    }
}
