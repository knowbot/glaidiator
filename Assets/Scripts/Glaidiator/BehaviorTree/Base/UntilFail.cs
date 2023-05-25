namespace Glaidiator.BehaviorTree.Base
{
    public class UntilFail : Decorator<Node>
    {

        public UntilFail() : base() { }
        public UntilFail(Node child) : base(child)
        {
            Child = child;
        }

        /**
         * reprocess/loop child node until child evaluates as failure,
         * then return success to break loop.
         */
        public override NodeState Evaluate()
        {
            // this node will be success when child fails
            while (state != NodeState.SUCCESS) 
            {
                switch (Child.Evaluate())
                {
                    case NodeState.FAILURE:
                        state = NodeState.SUCCESS;
                        return state;
                    case NodeState.SUCCESS:
                        state = NodeState.FAILURE;
                        continue;
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        continue;
                }
            }

            return state;
        }

        public override Node Clone()
        {
            return Child != null ? new UntilFail(Child.Clone()) : new UntilFail();
        }

        public override void Mutate()
        {
            Child?.Mutate();
        }

        public override Node Randomized()
        {
            if (Child == null) return new UntilFail(BTreeFactory.GetRandomNode().Randomized());
            var newNode = Clone() as AlwaysSucceed;
            newNode?.ReplaceChild(newNode.Child, newNode.Child.Randomized());
            return newNode;
        }
    }
}