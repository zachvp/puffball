using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CoreUtilities
{
    public static IEnumerator RunTask(Action task)
    {
        while (true)
        {
            task();
            yield return null;
        }
    }

    public static IEnumerator RepeatTask(float interval, Action task)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            task();
            yield return null;
        }
    }

    public static IEnumerator PostFixedUpdateTask(Action task)
    {
        yield return new WaitForFixedUpdate();
        task();
        yield return null;
    }

    public static IEnumerator DelayedTask(float delay, Action task)
    {
        yield return new WaitForSeconds(delay);
        task();
        yield return null;
    }

    // rounds given number to closest multiple of unit
    public static float RoundTo(float value, float unit)
    {
        return Mathf.Round(value / unit) * unit;
    }

    public static Vector2 RoundTo(Vector2 value, float unit)
    {
        return new Vector2(RoundTo(value.x, unit), RoundTo(value.y, unit));
    }

    public static bool Compare(float lhs, float rhs)
    {
        return Mathf.Abs(lhs - rhs) < CoreConstants.DEADZONE_FLOAT;
    }
}

public static class CoreConstants
{
    public const float DEADZONE_FLOAT = 0.01f;
    public const float DEADZONE_VELOCITY = 2;
    public const float UNIT_ROUND_POSITION = 1f / 16f;
    public const float THRESHOLD_DOT = 0.84f;
}

public static class Emitter
{
    public static void Send(Action handler)
    {
        // Temp variable for thread safety.
        var threadsafeHandler = handler;
        if (threadsafeHandler != null)
        {
            threadsafeHandler();
        }
    }

    public static void Send<T>(Action<T> eventHandler, T arg0)
    {
        // Temp variable for thread safety.
        var threadsafeHandler = eventHandler;
        if (threadsafeHandler != null)
        {
            threadsafeHandler(arg0);
        }
    }

    public static void Send<T, U>(Action<T, U> eventHandler, T arg0, U arg1)
    {
        // Temp variable for thread safety.
        var threadsafeHandler = eventHandler;
        if (threadsafeHandler != null)
        {
            threadsafeHandler(arg0, arg1);
        }
    }
}

public class Singleton<T> where T : new()
{
    public static T instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();

                // register for cleanup
                SceneManager.sceneUnloaded += (scene) =>
                {
                    _instance = default(T);
                };
            }

            return _instance;
        }
    }

    protected static T _instance;
}

public class Signals : Singleton<Signals>
{
    // Global notification definitions
    public Action<PCInputArgs> onPCCommand;
    public Action<PCInputCommandEmitter> onPCCommandEmitterSpawn;
}

[Serializable]
public struct VarWatch<T>
{
    public T value;

    [NonSerialized]
    public T oldValue;

    // Change event that sends (oldValue, newValue)
    public Action<T, T> onChanged;
    public Action<T, T> onUpdated;

    public void Update(T newValue)
    {
        value = newValue;

        Emitter.Send(onUpdated, oldValue, newValue);

        if (!oldValue.Equals(newValue))
        {
            Emitter.Send(onChanged, oldValue, newValue);
            oldValue = value;
        }
    }
}
