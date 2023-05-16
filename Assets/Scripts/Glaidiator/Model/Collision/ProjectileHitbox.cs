using Glaidiator.Model.Actions;
using UnityEngine;

namespace Glaidiator.Model.Collision
{
    public class ProjectileHitbox : Hitbox<Attack>
    {
        public float Range { get; }
        public float Speed { get; }
        private float _distance = 0f;
        public ProjectileHitbox(Collider2D collider, Character owner, float range, float speed, float lifetime = 0) 
            : base(collider, owner, lifetime)
        {
            Range = range;
            Speed = speed;
            Direction = Vector2.zero;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            if (_distance >= Range)
            {
                ToDestroy = true;
                return;
            }
            float d = Speed * deltaTime;
            _distance += d;
            Collider.Position += Direction * d;
      }
    }
}