using System.Collections.Generic;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Utils;

namespace Glaidiator.BehaviorTree.Base
{

    public class Randomizer : Decorator
    {
        
        public Randomizer() : base() {}

        public Randomizer(List<Node> children) : base(children) {}

        public Randomizer(Node child)
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
                clone = new Randomizer(Child.Clone());
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
                Child = EvolutionManager.GetNewRandomNode().Clone();
            }
        }

        public override Node Randomized()
        {
            return new Inverter(EvolutionManager.GetNewRandomNode().Randomized());
        }
    }
}
