using System.Numerics;
using Glaidiator.Model.Actions;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Glaidiator.Model.Collision
{
    public class WallHitbox : Hitbox<Wall>
    {
        public WallHitbox(Collider2D collider, Character owner) 
            : base(collider, owner, 0)
        {
            Direction = Vector2.zero;
        }
    }
}