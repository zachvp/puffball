using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ZCore
{
    public static class Constants
    {
        public const float DEADZONE_FLOAT_0 = 0.01f;
        public const float DEADZONE_FLOAT_1 = 0.02f;
        public const float DEADZONE_FLOAT_2 = 0.05f;
        public const float DEADZONE_FLOAT_3 = 0.1f;

        public const float DEADZONE_VELOCITY = 2;

        public const float THRESHOLD_DOT_INPUT = 0.84f;
        public const float THRESHOLD_DOT_PRECISE = 0.99f;
        public const float THRESHOLD_DOT_LOOSE = 0.7f;

        public const float UNIT_ROUND_POSITION = 1 / 32f;
        public const float UNIT_TIME_SLICE = 1 / 60f;

        public const string DEFAULT_MENU = "Custom/";

        public const string NAME_FILL_PREFIX = "fill"; // todo: move to puffball-specific constants file
        public const string NAME_OBJECT_VIS = "_visible";
        public const string NAME_OBJECT_COLL = "_collider";

        public const string CONTROL_SCHEME_KEYBOARD_MOUSE = "Keyboard&Mouse";
        public const string CONTROL_SCHEME_GAMEPAD = "Gamepad";

        public const int LAYER_PLAYER = 3;
        // intermediate values are built in
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
        public Action onScore;
    }

    public static class Emitter
    {
        public static void Send(Action handler)
        {
            // Temp variable for thread safety.
            var threadsafeHandler = handler;
            threadsafeHandler?.Invoke();
        }

        public static void Send<T>(Action<T> eventHandler, T arg0)
        {
            // Temp variable for thread safety.
            var threadsafeHandler = eventHandler;
            threadsafeHandler?.Invoke(arg0);
        }

        public static void Send<T, U>(Action<T, U> eventHandler, T arg0, U arg1)
        {
            // Temp variable for thread safety.
            var threadsafeHandler = eventHandler;
            threadsafeHandler?.Invoke(arg0, arg1);
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

    public class TrackMinMax
    {
        public float current { get; private set; }
        public float min { get; private set; }
        public float max { get; private set; }

        public TrackMinMax()
        {
            min = float.MaxValue;
            max = float.MinValue;
        }

        public void Update(float value)
        {
            current = value;
            min = Mathf.Min(min, value);
            max = Mathf.Max(max, value);
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
        public readonly T[] data;

        public int index { get; private set; }

        public BufferCircular(int size)
        {
            data = new T[size];
        }

        public void Add(T item)
        {
            data[index] = item;
            index = Next(index);
        }

        public int Next(int current)
        {
            return Index(current, 1);
        }

        public int Previous(int current)
        {
            return Index(current, -1);
        }

        public int Index(int current, int diff)
        {
            return CoreUtilities.Circular(current, data.Length, diff);
        }
    }

    public class BufferInterval<T> : BufferCircular<T>
    {
        public readonly float interval;
        public float timePrevious { get; private set; }

        public BufferInterval(int size, float interval) :
            base(size)
        {
            this.interval = interval;
        }

        public bool Add(T item, float time)
        {
            if (time - timePrevious > interval)
            {
                Add(item);
                timePrevious = time;
                return true;
            }
            return false;
        }
    }

    public class BufferQueue<T>
    {
        public readonly LinkedList<T> buffer = new LinkedList<T>();
        public readonly MonoBehaviour behavior;
        public readonly float lifetime; // todo: rename

        public BufferQueue(MonoBehaviour behavior, float lifetime)
        {
            this.behavior = behavior;
            this.lifetime = lifetime;
        }

        public void Add(T item)
        {
            var node = buffer.AddLast(item);

            behavior.StartCoroutine(CoreUtilities.TaskDelayed(lifetime, node, (n) =>
            {
                buffer.Remove(n);
            }));
        }
    }

    [Serializable]
    public struct BindingGameObject
    {
        public GameObject source;
        public GameObject target;
    }

    // TODO: refactor at 500 lines
}