using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

// todo: use ZCore namespace
public static class CoreUtilities
{
    #region Coroutines

    public static IEnumerator TaskContinuous(Action task)
    {
        while (true)
        {
            task();
            yield return null;
        }
    }

    public static IEnumerator TaskContinuous(Action task, Func<bool> isActive)
    {
        while (isActive())
        {
            task();
            yield return null;
        }
    }

    public static IEnumerator TaskRepeat(float interval, Action task)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            task();
            yield return null;
        }
    }

    public static IEnumerator TaskFixedUpdate(Action task)
    {
        yield return new WaitForFixedUpdate();
        task();
    }

    public static IEnumerator TaskFixedUpdate<T>(T arg0, Action<T> task)
    {
        yield return new WaitForFixedUpdate();
        task(arg0);
    }

    public static IEnumerator TaskDelayed(float delay, Action task)
    {
        yield return new WaitForSeconds(delay);
        task();
    }

    public static IEnumerator TaskDelayed<T>(float delay, T arg0, Action<T> task)
    {
        yield return new WaitForSeconds(delay);
        task(arg0);
    }

    public static IEnumerator TaskDelayed<T, U>(float delay, T arg0, U arg1, Action<T, U> task)
    {
        yield return new WaitForSeconds(delay);
        task(arg0, arg1);
    }
    #endregion

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
        return Mathf.Abs(lhs - rhs) < CoreConstants.DEADZONE_FLOAT_0;
    }

    #region Unity Object content duplication
    public static void CopyCollider(CircleCollider2D source, CircleCollider2D copy)
    {
        Debug.Assert(source != null && copy != null,
            $"At least one NULL ref passed to {nameof(CopyCollider)}");

        CopyColliderBase(source, copy);

        copy.radius = source.radius;
    }

    public static void CopyCollider(PolygonCollider2D source, PolygonCollider2D copy)
    {
        Debug.Assert(source != null && copy != null,
            $"At least one NULL ref passed to {nameof(CopyCollider)}");

        CopyColliderBase(source, copy);

        copy.autoTiling = source.autoTiling;
        copy.useDelaunayMesh = source.useDelaunayMesh;
        copy.points = source.points;
        copy.pathCount = source.pathCount;
    }

    public static void CopyCollider(CapsuleCollider2D source, CapsuleCollider2D copy)
    {
        Debug.Assert(source != null && copy != null,
            $"At least one NULL ref passed to {nameof(CopyCollider)}");

        CopyColliderBase(source, copy);

        copy.size = source.size;
        copy.direction = source.direction;
    }

    public static void CopyColliderBase(Collider2D source, Collider2D copy)
    {
        Debug.Assert(source != null && copy != null,
            $"At least one NULL ref passed to {nameof(CopyCollider)}");

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

    public static void CopyTransform(Transform source, Transform copy)
    {
        copy.position = source.position;
        copy.rotation = source.rotation;
        
        copy.localScale = source.localScale;
    }

    #endregion

    public static void Increment<T>(Dictionary<T, int> d, T key)
    {
        if (d.ContainsKey(key))
        {
            d[key] += 1;
        }
        else
        {
            d.Add(key, 1);
        }
    }

    public static bool Decrement<T>(Dictionary<T, int> d, T key)
    {
        if (d.ContainsKey(key))
        {
            d[key] -= 1;
            if (d[key] == 0)
            {
                d.Remove(key);
            }

            return true;
        }
        else
        {
            return false;
        }

    }

    #region GameObject utils

    public static void ForeachChild(Transform root, Action<Transform> callback)
    {
        for (var i = 0; i < root.childCount; i++)
        {
            callback(root.GetChild(i));
        }
    }

    public static GameObject FindChild(Transform source, string prefix)
    {
        GameObject result = null;

        ForeachChild(source, (child) =>
        {
            if (child.name.StartsWith(prefix))
            {
                Debug.Assert(result == null, $"multiple children {prefix} found on {nameof(GameObject)} {source}");

                result = child.gameObject;
            }
        });

        return result;
    }

    public static void DestroyDependents(GameObject target)
    {
        if (target)
        {
            while (target.transform.childCount > 0)
            {
                UnityEngine.Object.DestroyImmediate(target.transform.GetChild(0).gameObject);
            }

            UnityEngine.Object.DestroyImmediate(target);
        }
    }

    #endregion
}

public static class CoreConstants
{
    public const float DEADZONE_FLOAT_0 = 0.01f;
    public const float DEADZONE_FLOAT_1 = 0.05f;
    public const float DEADZONE_FLOAT_2 = 0.1f;
    public const float DEADZONE_VELOCITY = 2;
    public const float UNIT_ROUND_POSITION = 1f / 32f;
    public const float THRESHOLD_DOT_INPUT = 0.84f;
    public const float THRESHOLD_DOT_PRECISE = 0.99f;
    public const float THRESHOLD_DOT_LOOSE = 0.7f;

    public const string DEFAULT_MENU = "Custom/";

    public const string NAME_FILL_PREFIX = "fill";
    public const string NAME_OBJECT_VIS = "_visible";
    public const string NAME_OBJECT_COLL = "_collider";

    public const int LAYER_PLAYER = 3;
    public const int LAYER_OBSTACLE = 6;
    public const int LAYER_ACTOR = 7;
    public const int LAYER_TRIGGER = 8;
    public const int LAYER_PICKUP = 9;
    public const int LAYER_PROP = 10;
}

public class Signals : Singleton<Signals>
{
    // Global notification definitions
    public Action<PCInputArgs> onPCCommand;
    public Action<PCInputCommandEmitter> onPCCommandEmitterSpawn;
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

public class BufferInterval<T> : BufferCircular<T>
{
    public readonly float interval;
    public float timePrevious { get; private set; }

    public BufferInterval(int inSize, float inInterval) :
        base(inSize)
    {
        interval = inInterval;
    }

    public void Add(T item, float time)
    {
        if (time - timePrevious > interval)
        {
            Add(item);
            timePrevious = time;
        }
    }
}

[Serializable]
public struct BindingGameObject
{
    public GameObject source;
    public GameObject target;
}

[Serializable]
public struct Range
{
    public float min;
    public float max;
}

// TODO: refactor at 500 lines