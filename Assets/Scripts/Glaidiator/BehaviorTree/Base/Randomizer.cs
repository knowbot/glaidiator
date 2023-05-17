using Glaidiator.Utils;

namespace Glaidiator.BehaviorTree.Base
{

    public class Randomizer : Decorator<Composite>
    {
        public Randomizer() : base() {}

        public Randomizer(Composite child)
        {
            Child = child;
        }
        
        public override NodeState Evaluate()
        {
            Child.Children.Shuffle();
            state = Child.Evaluate();
            return state;
        }

        public override Node Clone()
        {
            Node clone;
            if (Child != null)
            {
                clone = new Randomizer((Composite)Child.Clone());
            }
            else
            {
                clone = new Randomizer();
            }
            
            return clone;
        }

        public override void Mutate()
        {
            if (Child == null)
            {
                // TODO: replace with composite
                // //Child = EvolutionManager.GetNewRandomNode().Clone();
            }
        }

        public override Node Randomized()
        {
            return new Inverter(EvolutionManager.GetNewRandomNode().Randomized());
        }
    }
}
