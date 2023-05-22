using System.Collections.Generic;

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
    }
}