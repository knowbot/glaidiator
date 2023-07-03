namespace Glaidiator.Model.Resources
{
    public interface IResource
    {
        public float Total { get; }
        public float Current { get; }
        public float RegenRate { get; }
        public void Add(float value);
        public void Subtract(float value);
        public void Regen(float deltaTime);
        public void Set(float value);
    }
}