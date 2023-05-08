using Collider2D = Glaidiator.Model.Collision.Collider2D;

namespace Glaidiator.Model.Collision
{
    public interface IHitbox
    {
        public bool Active { get; }
        public Character Owner { get; }
        Collider2D Collider { get; }
        bool ToDestroy { get; }
        public IHitbox Create();
        public void Destroy();
        public void Register();
        public void Deregister();
        public void Update(float deltaTime);
    }
}