using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Commons
{

    public static class Commons
    {
        public static Coroutine SetTimeout(this MonoBehaviour mono, Action action, float delay)
        {
            return mono.StartCoroutine(ISetTimeout(action, delay));
        }

        public static IEnumerator ISetTimeout(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action();
        }

        public static Coroutine WaitNewFrame(this MonoBehaviour mono, Action action)
        {
            return mono.StartCoroutine(IWaitNewFrame(action));
        }

        public static IEnumerator IWaitNewFrame(Action action)
        {
            yield return null;
            action();
        }

    }
    [Serializable]
    public class SerializedTuple<T1, T2>
    {
        public T1 Item1;
        public T2 Item2;

        public SerializedTuple(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }

    [Serializable]
    public class SerializedTuple<T1, T2, T3>
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;

        public SerializedTuple(T1 item1, T2 item2, T3 item3)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
        }
    }

    public static class CommonMath
    {
        public static Vector3 RotateAroundY(this Vector3 v, Vector3 axis)
        {
            float angle = v.ToVector2().CalculateAngleTo(axis.ToVector2());
            return Quaternion.Euler(0, angle, 0) * v;

        }

        public static float CalculateAngleTo(this Vector2 v, Vector2 axis)
        {
            float angle = Vector2.Angle(v, axis); // Calculate angle
            Vector3 cross = Vector3.Cross(v, axis); // Calculate cross
            if (cross.y < 0) angle = -angle; // If cross is -y then angle is negative
            return angle;
        }

        public static float CalculateAngleTo(this Vector3 v, Vector3 axis)
        {
            float angle = Vector3.Angle(v, axis); // Calculate angle
            Vector3 cross = Vector3.Cross(v, axis); // Calculate cross
            if (cross.y < 0) angle = -angle; // If cross is -y then angle is negative
            return angle;
        }


        public static Vector2 ToVector2(this Vector3 v) => new Vector2(v.x, v.z);

        public static float SeparateRandom(float min, float max, int count)
        {
            float range = max - min;
            float step = range / count;
            return Random.Range(0, count) * step + min;
        }
    }
}
