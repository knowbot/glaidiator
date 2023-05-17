namespace Glaidiator.BehaviorTree.Base
{

    public class AlwaysSucceed : Decorator<Node>
    {

        public AlwaysSucceed() : base() { }

        public AlwaysSucceed(Node child)
        {
            Child = child;
        }

        public override NodeState Evaluate()
        {
            //_child.Evaluate();

            switch (Child.Evaluate())
            {
                case NodeState.FAILURE:
                    state = NodeState.SUCCESS;
                    return state;
                case NodeState.SUCCESS:
                    state = NodeState.SUCCESS;
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
                clone = new AlwaysSucceed(Child.Clone());
            }
            else
            {
                clone = new AlwaysSucceed();
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
            return new AlwaysSucceed(EvolutionManager.GetNewRandomNode().Randomized());
        }
    }
    
    
}