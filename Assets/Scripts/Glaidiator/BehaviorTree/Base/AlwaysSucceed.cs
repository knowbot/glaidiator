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
            Child ??= BTreeFactory.GetRandomNode().Randomized();
        }

        public override Node Randomized()
        {
            if (Child == null) return new UntilFail(BTreeFactory.GetRandomNode().Randomized());
            var newNode = Clone() as UntilFail;
            newNode?.ReplaceChild(newNode.Child, newNode.Child.Randomized());
            return newNode;
        }
    }
    
    
}