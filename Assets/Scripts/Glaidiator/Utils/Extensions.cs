using System.Collections.Generic;
using UnityEngine;

namespace Glaidiator.Utils
{
    public static class Extensions
    {
        public static void Shuffle<T>(this IList<T> values)
        {
            for (int i = values.Count - 1; i > 0; i--)
            {
                int k = MathStuff.Rand.NextInt(i + 1);
                (values[k], values[i]) = (values[i], values[k]);
            }
        }
        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

            float tx = v.x;
            float ty = v.y;
            v.x = (cos * tx) - (sin * ty);
            v.y = (sin * tx) + (cos * ty);
            return v;
        }
        
    }
}