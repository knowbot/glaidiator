using System;

namespace Glaidiator.BehaviorTree.Base
{

    public class Inverter : Decorator<Node>
    {
        public Inverter() {}
        
        public Inverter(Node child) : base(child) { }
        
        public override NodeState Evaluate()
        {
            state = Child.Evaluate() switch
            {
                NodeState.FAILURE => NodeState.SUCCESS,
                NodeState.SUCCESS => NodeState.FAILURE,
                NodeState.RUNNING => NodeState.RUNNING,
                _ => NodeState.FAILURE
            };
            return state;
        }

        public override Node Clone()
        {
            return Child != null ? new Inverter(Child.Clone()) : new Inverter();;
        }

        public override void Mutate()
        {
            Child?.Mutate();
        }

        public override Node Randomized()
        {
            return new Inverter(BTreeFactory.GetRandomNode().Randomized());
        }
    }
}
