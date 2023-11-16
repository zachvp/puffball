using System;
using UnityEngine;

namespace ZCore
{
    [Flags]
    public enum Direction2D
    {
        NONE  = 0,
        LEFT  = 1 << 1,
        RIGHT = 1 << 2,
        DOWN  = 1 << 3,
        UP    = 1 << 4
    }

    public static class EnumHelper
    {
        public static string GetStringID(Enum value)
        {
            Debug.Assert(value.GetType() == typeof(int), $"unhandled type '{value.GetType()}' for enum value '{value}'");
            
            return Convert.ToInt32(value).ToString();
        }

        public static Direction2D FromBool(bool left, bool right, bool down, bool up)
        {
            var result = Direction2D.NONE;

            result |= left  ? Direction2D.LEFT   : Direction2D.NONE;
            result |= right ? Direction2D.RIGHT  : Direction2D.NONE;
            result |= down  ? Direction2D.DOWN   : Direction2D.NONE;
            result |= up    ? Direction2D.UP     : Direction2D.NONE;

            return result;
        }
    }
}