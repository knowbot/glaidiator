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
            return Child != null ? new AlwaysSucceed(Child.Clone()) : new AlwaysSucceed();
        }

        public override void Mutate()
        {
            Child?.Mutate();
        }

        public override Node Randomized()
        {
            return new AlwaysSucceed(BTreeFactory.GetRandomPrototype().Randomized());
        }
    }
    
    
}