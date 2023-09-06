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
    BALL = 1 << 1,
    PLAYER = 1 << 2
}

public static class CoreActionMap
{
    public enum Type
    {
        NONE,
        PLAYER
    }

    public static class Player
    {
        public const string START     = "Start";
        public const string JUMP      = "Jump";
        public const string MOVE      = "Move";
        public const string MOVE_HAND = "Move Hand";
        public const string GRIP      = "Grip";
        public const string THROW     = "Throw";

        public enum Action : Int32
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

    public static Player.Action GetPlayerAction(string name)
    {
        var result = Player.Action.NONE;
        var map = new Dictionary<string, Player.Action>
        {
            { Player.START,     Player.Action.START },
            { Player.JUMP,      Player.Action.JUMP },
            { Player.MOVE,      Player.Action.MOVE },
            { Player.MOVE_HAND, Player.Action.MOVE_HAND },
            { Player.GRIP,      Player.Action.GRIP },
            { Player.THROW,     Player.Action.THROW },
        };

        if (map.ContainsKey(name))
        {
            result = map[name];
        }

        return result;
    }

    public static Type GetActionMap(string name)
    {
        var result = Type.NONE;
        var map = new Dictionary<string, Type>
        {
            { "player", Type.PLAYER },
        };

        if (map.ContainsKey(name.ToLower()))
        {
            result = map[name.ToLower()];
        }

        return result;
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
}
