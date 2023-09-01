using System;
using System.Collections.Generic;

[Flags]
public enum Direction2D
{
    NONE = 0,
    UP = 1 << 0,
    DOWN = 1 << 1,
    LEFT = 1 << 2,
    RIGHT = 1 << 3
}

[Flags]
public enum Trait
{
    NONE = 0,
    PICKUP = 1 << 0,
    BALL = 1 << 1
}

public static class CoreActionMap
{
    public enum Type
    {
        NONE,
        PLAYER
    }

    public enum Player : Int32
    {
        NONE,

        START,
        JUMP,
        MOVE,
        MOVE_HAND,
        GRIP,
        THROW // todo: rename to be more generic (e.g. HAND_ACTION); also rename the action map asset
    }
}

public static class EnumHelper
{
    public static string GetStringID(Enum value)
    {
        return Convert.ToInt32(value).ToString();
    }

    public static Direction2D FromBool(bool left, bool right, bool down, bool up)
    {
        var result = Direction2D.NONE;

        result |= left ? Direction2D.LEFT : Direction2D.NONE;
        result |= right ? Direction2D.RIGHT : Direction2D.NONE;
        result |= down ? Direction2D.DOWN : Direction2D.NONE;
        result |= up ? Direction2D.UP : Direction2D.NONE;

        return result;
    }

    public static CoreActionMap.Player GetPlayerAction(string name)
    {
        var result = CoreActionMap.Player.NONE;
        var map = new Dictionary<string, CoreActionMap.Player>
        {
            { "start", CoreActionMap.Player.START },
            { "jump", CoreActionMap.Player.JUMP },
            { "move", CoreActionMap.Player.MOVE },
            { "move hand", CoreActionMap.Player.MOVE_HAND },
            { "grip", CoreActionMap.Player.GRIP },
            { "throw", CoreActionMap.Player.THROW },
        };

        if (map.ContainsKey(name.ToLower()))
        {
            result = map[name.ToLower()];
        }

        return result;
    }

    public static CoreActionMap.Type GetActionMap(string name)
    {
        var result = CoreActionMap.Type.NONE;
        var map = new Dictionary<string, CoreActionMap.Type>
        {
            { "player", CoreActionMap.Type.PLAYER },
        };

        if (map.ContainsKey(name.ToLower()))
        {
            result = map[name.ToLower()];
        }

        return result;
    }
}
