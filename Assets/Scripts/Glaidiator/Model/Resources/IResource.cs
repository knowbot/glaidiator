namespace Glaidiator.Model
{
    public interface IResource
    {
        public float Total { get; }
        public void Add();
        public void Remove();
    }
}