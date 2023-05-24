using Glaidiator.Model;
using Glaidiator.Utils;

namespace Glaidiator.Presenter
{
    public class RandomInputProvider : IInputProvider
    {
        public Input Inputs { get; private set; }
        public Input RandomInputs()
        {
            return new Input()
            {
                attackLight = MathStuff.Rand.NextBool(),
                attackHeavy = MathStuff.Rand.NextBool(),
                attackRanged = MathStuff.Rand.NextBool(),
                block = MathStuff.Rand.NextBool(),
                dodge = MathStuff.Rand.NextBool(),
                move = MathStuff.Get8DDirection(MathStuff.Rand.NextFloat(), MathStuff.Rand.NextFloat())
            };
        }
        

        public Input GetInputs()
        {
            Inputs = RandomInputs();
            return Inputs;
        }
    }
}