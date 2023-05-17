namespace Glaidiator.BehaviorTree.Base
{

    public class Inverter : Decorator<Node>
    {
        
        public Inverter() : base() {}
        
        public Inverter(Node child)
        {
            Child = child;
        }
        
        public override NodeState Evaluate()
        {

            switch (Child.Evaluate())
            {
                case NodeState.FAILURE:
                    state = NodeState.SUCCESS;
                    return state;
                case NodeState.SUCCESS:
                    state = NodeState.FAILURE;
                    return state;
                case NodeState.RUNNING:
                    state = NodeState.RUNNING;
                    return state;
            }

            state = NodeState.SUCCESS;
            return state;
        }

        public override Node Clone()
        {
            Node clone;
            if (Child != null)
            {
                clone = new Inverter(Child.Clone());
            }
            else
            {
                clone = new Inverter();
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
