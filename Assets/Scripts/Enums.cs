using System;
using System.Collections.Generic;

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
        public const string START           = "Start";
        public const string JUMP            = "Jump";
        public const string MOVE            = "Move";
        public const string MOVE_HAND       = "Move Hand";
        public const string GRIP            = "Grip";
        public const string HAND_ACTION     = "Hand Action";

        public enum Action : Int32
        {
            NONE,

            START,
            JUMP,
            MOVE,
            MOVE_HAND,
            GRIP,
            HAND_ACTION
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
            { Player.HAND_ACTION,     Player.Action.HAND_ACTION },
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
