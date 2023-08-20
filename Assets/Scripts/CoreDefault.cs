using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// todo: use ZCore namespace
public static class CoreUtilities
{
    private static int testRecurse;

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
    }

    public static IEnumerator DelayedTask(float delay, Action task)
    {
        yield return new WaitForSeconds(delay);
        task();
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

    public static void CopyCollider(CircleCollider2D source, CircleCollider2D copy)
    {
        Debug.Log($"circle collider called for copy");

        Debug.Assert(source != null && copy != null,
            $"At least one NULL ref passed to {nameof(CopyCollider)}");

        CopyColliderBase(source, copy);

        copy.radius = source.radius;
    }

    public static void CopyCollider(PolygonCollider2D source, PolygonCollider2D copy)
    {
        CopyColliderBase(source, copy);

        copy.autoTiling = source.autoTiling;
        copy.useDelaunayMesh = source.useDelaunayMesh;
        copy.points = source.points;
        copy.pathCount = source.pathCount;
    }

    public static void CopyCollider(CapsuleCollider2D source, CapsuleCollider2D copy)
    {
        CopyColliderBase(source, copy);

        copy.size = source.size;
        copy.direction = source.direction;
    }

    public static void CopyColliderBase(Collider2D source, Collider2D copy)
    {
        // misc
        copy.isTrigger = source.isTrigger;
        copy.offset = (Vector2)source.transform.position + source.offset;
        copy.sharedMaterial = source.sharedMaterial;
        copy.usedByEffector = source.usedByEffector;

        // layer overrides
        copy.layerOverridePriority = source.layerOverridePriority;
        copy.includeLayers = source.includeLayers;
        copy.excludeLayers = source.excludeLayers;
        copy.forceSendLayers = source.forceSendLayers;
        copy.forceReceiveLayers = source.forceReceiveLayers;
        copy.contactCaptureLayers = source.contactCaptureLayers;
        copy.callbackLayers = source.callbackLayers;
    }
}

public static class CoreConstants
{
    public const float DEADZONE_FLOAT = 0.01f;
    public const float DEADZONE_VELOCITY = 2;
    public const float UNIT_ROUND_POSITION = 1f / 16f;
    public const float THRESHOLD_DOT_INPUT = 0.84f;
    public const float THRESHOLD_DOT_PRECISE = 0.99f;
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

// Used for cases in which the random call will be called repeatedly in a single frame/within a short time
public struct RandomInstant
{
    public int seed;

    public float Range(float minInclusive, float minExcluse)
    {
        var result = UnityEngine.Random.Range(minInclusive, minExcluse);

        UnityEngine.Random.InitState(seed);
        seed += 1 + (int)(seed % result);

        return result;
    }
}

public class BufferCircular<T>
{
    public readonly T[] buffer;
    public readonly int size;

    private int index;

    public BufferCircular(int inSize)
    {
        buffer = new T[inSize];
        size = inSize;
    }

    public void Add(T item)
    {
        buffer[index] = item;
        index = (index + 1) % size;
    }
}
