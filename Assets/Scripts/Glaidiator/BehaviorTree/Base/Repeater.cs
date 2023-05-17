using UnityEngine;

namespace Glaidiator.BehaviorTree.Base
{
    public class Repeater : Decorator<Node>
    {
        private int _count = -1;

        public Repeater() : base() { }

        public Repeater(int count) : base()
        {
            _count = count;
        }

        public Repeater(Node child) : base(child) // forever
        {
            Child = child;
        }

        public Repeater(Node child, int count) : base(child)
        {
            Child = child;
            _count = count;
        }

        public override NodeState Evaluate()
        {
            int evalCount = _count;
            
            state = NodeState.RUNNING;
            NodeState childState = state;
            
            if (evalCount == -1)
            {
                while (true) // TODO: less dirty solution?
                {
                    childState = Child.Evaluate();
                }
            }
            
            if (evalCount > 0)
            {
                while (evalCount > 0)
                {
                    childState = Child.Evaluate();
                    evalCount--;
                }

                state = childState;
            }
            

            return state;
        }

        public override Node Clone()
        {
            Node clone;
            if (_count < 1)
            {
                if (Child != null)
                {
                    clone = new Repeater(Child.Clone());
                }
                else
                {
                    clone = new Repeater();
                }
            }
            else
            {
                if (Child != null)
                {
                    clone = new Repeater(Child.Clone(), _count);
                }
                else
                {
                    clone = new Repeater();
                }
            }
            

            return clone;
        }

        public override void Mutate()
        {
            if (_count == -1) return; // doesn't make sense to change a forever repeater?
            int countDiff = Random.Range(-5, 5);
            int newCount = _count + countDiff;
            if (newCount > 0)
            {
                _count = newCount;
            }
        }

        public override Node Randomized()
        {
            Node newChild = EvolutionManager.GetNewRandomNode();
            if (_count == -1)
            {
                return new Repeater(newChild);
            }
            return new Repeater(newChild, Random.Range(1, 6));
        }
    }
}