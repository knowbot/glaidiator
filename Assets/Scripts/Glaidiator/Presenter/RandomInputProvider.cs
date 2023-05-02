using UnityEditor;
using UnityEngine;
using MathUtils = Glaidiator.Model.Utils.MathUtils;

namespace Glaidiator.Presenter
{
    public class RandomInputProvider : IInputProvider
    {
        public Input Inputs { get; private set; }
        public Input RandomInputs()
        {
            var rnd = new Unity.Mathematics.Random(42069);
            return new Input()
            {
                attackLight = rnd.NextBool(),
                attackHeavy = rnd.NextBool(),
                attackRanged = rnd.NextBool(),
                block = rnd.NextBool(),
                dodge = rnd.NextBool(),
                move = MathUtils.Get8DDirection(rnd.NextFloat(), rnd.NextFloat())
            };
        }
        

        public Input GetInputs()
        {
            Inputs = RandomInputs();
            return Inputs;
        }
    }
}