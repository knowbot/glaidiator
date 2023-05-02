using Glaidiator.Model.Collision;
using UnityEngine;
using Collider2D = Glaidiator.Model.Collision.Collider2D;

namespace Glaidiator.Model.Actions
{
    public class AttackRanged : Attack
    {
        public Vector3 Direction { get; set; }

        public AttackRanged(ActionInfo action, ProjectileHitbox hitbox, float damage, float cooldown = 0, float delay = 0) 
            : base(action, hitbox, damage, cooldown, delay)
        {
        }
    }
}