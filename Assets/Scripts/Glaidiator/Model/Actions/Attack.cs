using System;
using Glaidiator.Model.Actions;
using Unity.VisualScripting;
using UnityEngine;
using Glaidiator.Model.Collision;
using Timer = Glaidiator.Model.Utils.Timer;
using Collider2D = Glaidiator.Model.Collision.Collider2D;

namespace Glaidiator.Model.Actions
{
    public class Attack : IAction, ICooldown
    {
        public ActionInfo Action { get; }
        public float Damage;
        public Timer Cooldown { get; }
        public string Name { get; }
        public Hitbox<Attack> Hitbox { get; private set; }



        // TODO: add hitbox info
        public Attack(ActionInfo action, Hitbox<Attack> hitbox, float damage, float cooldownDuration = 0f)
        {
            Action = action;
            Damage = damage;
            Name = action.Name;
            Cooldown = new Timer(cooldownDuration);
            Hitbox = hitbox;
            Hitbox.Origin = this;
        }

        public ICooldown SetOnCooldown()
        {
            Cooldown.Reset();
            return this;
        }

        public void SpawnHitbox(Vector2 direction)
        {
            Hitbox.Direction = direction;
            Hitbox.Create();
        }
    }
}