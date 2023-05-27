using UnityEngine;

namespace Glaidiator.BehaviorTree.Base
{
    public class Repeater : Decorator<Node>
    {
        private int _count;

        public Repeater() : base() { }

        public Repeater(int count) : base()
        {
            _count = count;
        }

        public Repeater(Node child, int count) : base(child)
        {
            _count = count;
        }

        public override NodeState Evaluate()
        {
            int evalCount = _count;
            
            state = NodeState.RUNNING;
            NodeState childState = state;
            
            while (evalCount > 0)
            {
                childState = Child.Evaluate();
                evalCount--;
            }
            state = childState;
            return childState;
        }

        public override Node Clone()
        {
            return Child != null ? new Repeater(Child.Clone(), _count) : new Repeater();;
        }

        public override void Mutate()
        {
            int countDiff = Random.Range(-(_count - 1), 5);
            _count += countDiff;
        }

        public override Node Randomized()
        {
            Node newChild = BTreeFactory.GetRandomNode().Randomized();
            return new Repeater(newChild, Random.Range(1, 10));
        }
    }
}