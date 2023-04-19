using System.Numerics;
using Glaidiator.Model.Actions;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Glaidiator.Model.Collision
{
    public class CharacterHitbox : Hitbox<Character>
    {
        public CharacterHitbox(Collider2D collider, Character owner, float lifetime = 0) 
            : base(collider, owner, lifetime)
        { }

        public override void Update(float deltaTime)
        {
            Collider.Center = Owner.Movement.Position.xz();
        }
    }
}