namespace Glaidiator.BehaviorTree.Base
{

    public class Inverter : Decorator<Node>
    {
        public Inverter() {}
        
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
            return Child != null ? new Inverter(Child.Clone()) : new Inverter();;
        }

        public override void Mutate()
        {
            Child ??= EvoManager.Instance.GetRandomNode().Clone();
        }

        public override Node Randomized()
        {
            if (Child == null) return new Inverter(EvoManager.Instance.GetRandomNode().Randomized());
            var newNode = Clone() as Inverter;
            newNode?.ReplaceChild(newNode.Child, newNode.Child.Randomized());
            return newNode;
        }
    }
}
