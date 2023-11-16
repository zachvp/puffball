using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace ZCore
{
    public static class CoreUtilities
    {
        #region Coroutines & Collection Utils

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

        #endregion

        #region Mathy
        // rounds given number to closest multiple of unit
        public static float RoundTo(float value, float unit)
        {
            return Mathf.Round(value / unit) * unit;
        }

        public static Vector2 RoundTo(Vector2 value, float unit)
        {
            return new Vector2(RoundTo(value.x, unit), RoundTo(value.y, unit));
        }

        public static bool LayerExistsInMask(int layerIndex, LayerMask mask)
        {
            return (mask.value & (1 << layerIndex)) > 0;
        }

        public static Vector2 SetX(Vector2 source, float value)
        {
            var result = source;
            result.x = value;

            return result;
        }

        public static Vector2 SetY(Vector2 source, float value)
        {
            var result = source;
            result.y = value;

            return result;
        }

        public static Vector2 Abs(Vector2 source)
        {
            var result = source;

            result.x = Mathf.Abs(result.x);
            result.y = Math.Abs(result.y);

            return result;
        }

        public static Vector2 AbsDiff(Vector2 lhs, Vector2 rhs)
        {
            var result = lhs;

            result.x = Mathf.Abs(lhs.x - rhs.x);
            result.y = Mathf.Abs(lhs.y - rhs.y);

            return result;
        }

        public static Vector2 ScreenToWorld(Camera camera, Vector2 source)
        {
            return (Vector2)camera.ScreenToWorldPoint(source);
        }

        public static void DrawScreenLine(Camera camera, Vector2 screenPosStart, Vector2 screenPosEnd)
        {
            Debug.DrawLine(ScreenToWorld(camera, screenPosStart),
                           ScreenToWorld(camera, screenPosEnd),
                           Color.blue,
                           0.2f);
        }

        public static int Circular(int current, int size)
        {
            Debug.Assert(size > 0, $"invalid value for {nameof(size)}: {size}");

            return (current % size + size) % size;
        }

        public static int Circular(int current, int size, int diff)
        {
            return Circular(current + diff, size);
        }

        #endregion

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

        #region GameObject utils

        public static GameObject FindRoot(GameObject source)
        {
            var root = source;

            while (root.transform.parent)
            {
                root = root.transform.parent.gameObject;
            }

            return root;
        }

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

        public static string NameSpecific(GameObject source)
        {
            if (source.transform.parent)
            {
                return $"{source.transform.parent.name}.{source.name}";
            }
            else
            {
                return $"{source.name}";
            }
        }

        public static string NameSpecific(Component source)
        {
            return NameSpecific(source.gameObject);
        }

        #endregion
    }
}