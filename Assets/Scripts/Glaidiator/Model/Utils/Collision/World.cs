using System.Collections.Generic;

namespace Glaidiator.Model.Utils.Collision
{
    public class World
    {
        private List<Collider2D> _colliders;

        public void AddCollider(Collider2D collider)
        {
            _colliders.Add(collider);
        }
        public void RemoveCollider(Collider2D collider)
        {
            _colliders.Remove(collider);
        }
    }
}