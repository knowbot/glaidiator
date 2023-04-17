using Glaidiator.Model.Utils.Collision;

namespace Glaidiator.Model.Actions.Interfaces
{
    public interface IHitbox
    {
        public Collider2D Hitbox { get; }
    }
}