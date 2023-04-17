using Collider2D = Glaidiator.Model.Collision.Collider2D;

namespace Glaidiator.Model.Collision
{
    public interface IHitbox
    {
        Collider2D Collider { get; }
        public void Create();
        public void Destroy();
        public void Update(float deltaTime);
    }
}