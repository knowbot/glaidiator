using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Repeater : Decorator
    {
        private int _count = -1;
        
        public Repeater(Node child) // forever
        {
            _child = child;
        }

        public Repeater(Node child, int count)
        {
            _child = child;
            _count = count;
        }

        public override NodeState Evaluate()
        {
            int evalCount = _count;
            
            state = NodeState.RUNNING;
            NodeState childState = state;
            
            if (evalCount == -1)
            {
                while (true) // FIXME: less dirty solution?
                {
                    childState = _child.Evaluate();
                }
            }
            
            if (evalCount > 0)
            {
                while (evalCount > 0)
                {
                    childState = _child.Evaluate();
                    evalCount--;
                }

                state = childState;
            }
            

            return state;
        }

        public override Node Clone()
        {
            Node clone = new Repeater(_child.Clone(), _count);
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