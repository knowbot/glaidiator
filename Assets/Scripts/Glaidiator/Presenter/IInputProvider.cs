namespace Glaidiator
{
    public interface IInputProvider
    {
        public Input Inputs { get; }
        public Input GetInputs();
    }
}