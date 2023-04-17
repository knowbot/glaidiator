using System;
using Glaidiator.Model.Actions.Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using Collider2D = Glaidiator.Model.Utils.Collision.Collider2D;
using Timer = Glaidiator.Model.Utils.Timer;

namespace Glaidiator.Model.Actions
{
    public class Attack : IAction, ICooldown, IHitbox
    {
        public ActionInfo Action { get; }
        public float Damage;
        public string Name { get; }
        public Timer Cooldown { get; }
        public Collider2D Hitbox { get; set; }

        // TODO: add hitbox info
        public Attack(ActionInfo action, float damage, float cooldownDuration = 0f)
        {
            Action = action;
            Damage = damage;
            Name = action.Name;
            Cooldown = new Timer(cooldownDuration);
        }

        public void EnableHitbox(Character owner)
        {
            Hitbox.Center = owner.Movement.Position.xz() + (owner.Movement.Rotation * Hitbox.Offset.x0y()).xz();
            Hitbox.Register();
        }

        public void DisableHitbox()
        {
            Hitbox.Deregister();
        }

        public ICooldown SetOnCooldown()
        {
            Cooldown.Reset();
            return this;
        }
    }
}