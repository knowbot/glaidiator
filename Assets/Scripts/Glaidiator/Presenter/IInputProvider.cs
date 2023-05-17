using Glaidiator.Model;

namespace Glaidiator.Presenter
{
    public interface IInputProvider
    {
        public Input Inputs { get; }
        public Input GetInputs();
    }
}