using UnityEngine;

namespace Glaidiator.Model.Utils.Collision
{
    public struct Bounds
    {
        public Vector2 Center;
        public Vector2 Size;

        public Bounds(Vector2 center, Vector2 size)
        {
            Center = center;
            Size = size;
        }

        public Vector2 Extents => Size / 2;
        public Vector2 Max => Center + Extents;
        public Vector2 Min => Center - Extents;
    }
}