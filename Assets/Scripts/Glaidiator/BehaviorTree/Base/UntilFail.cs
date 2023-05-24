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
            Node clone;
            if (Child != null)
            {
                clone = new UntilFail(Child.Clone());
            }
            else
            {
                clone = new UntilFail();
            }
            return clone;
        }

        public override void Mutate()
        {
            if (Child == null)
            {
                Child = EvoManager.Instance.GetRandomNode().Clone();
            }
        }

        public override Node Randomized()
        {
            if (Child == null) return new UntilFail(EvoManager.Instance.GetRandomNode().Randomized());
            var newNode = Clone() as AlwaysSucceed;
            newNode?.ReplaceChild(newNode.Child, newNode.Child.Randomized());
            return newNode;
        }
    }
}