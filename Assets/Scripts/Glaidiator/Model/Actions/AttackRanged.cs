using UnityEngine;

namespace Glaidiator.Model.Actions
{
    public class AttackRanged : Attack
    {
        private float _distance = 0f;
        public float Range { get; }
        public float Speed { get; }
        public Vector3 Direction { get; set; }

        public AttackRanged(ActionInfo action, float damage, float range, float speed,  float cooldownDuration = 0) 
            : base(action, damage, cooldownDuration)
        {
            Range = range;
            Speed = speed;
        }

        public void Tick(float deltaTime)
        {
            if (Hitbox is null) return;
            if (_distance > Range)
            {
                Hitbox.Destroy();
                Hitbox = null;
                return;
            }
            
            Hitbox.Center += Speed * Direction.xz() * deltaTime;
            _distance += Speed * deltaTime;
        }
    }
}