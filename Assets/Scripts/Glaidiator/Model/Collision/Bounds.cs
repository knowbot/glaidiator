using UnityEngine;

namespace Glaidiator.Model.Collision
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

    
    }
}