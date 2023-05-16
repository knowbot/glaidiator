namespace Glaidiator.Model.Collision
{
    public class CharacterHitbox : Hitbox<Character>
    {
        public CharacterHitbox(Collider2D collider, Character owner, float lifetime = 0) 
            : base(collider, owner, lifetime)
        { }

        public override void Update(float deltaTime)
        {
            Collider.Position = Owner.Movement.Position.xz();
        }
    }
}