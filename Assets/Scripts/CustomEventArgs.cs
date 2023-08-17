using UnityEngine;
using System;
using UnityEngine.InputSystem;

public struct InputButtonArgs
{
    public int playerID;
    public InputActionPhase phase;
    public CoreActionMap.Player action;
}

[Serializable]
public struct InputAxis1DArgs
{
    public int playerID;
    public CoreActionMap.Player action;
    public int axis;
}

[Serializable]
public struct InputAxis2DArgs
{
    public int playerID;
    public CoreActionMap.Player action;
    public Vector2 axis;
}

[Serializable]
public struct PCInputArgs
{
    public CoreActionMap.Player type;
    public int playerIndex;
    public Vector2 vVec2;
    public float vFloat;
    public bool vBool;

    public override string ToString()
    {
        return JsonUtility.ToJson(this);
        //return $"{nameof(type)}: {type} | {nameof(vVec2)}: {vVec2} | {nameof(vFloat)}: {vFloat} | {nameof(vBool)}: {vBool}";
    }
}
