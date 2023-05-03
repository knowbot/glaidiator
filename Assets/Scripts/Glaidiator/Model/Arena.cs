using UnityEngine;

namespace Glaidiator.Model
{
    public static class Arena
    {
        public static Vector2 Size = new Vector2(30f, 30f);
        public static float MaxSize = Mathf.Max(Size.x, Size.y);
    }
}