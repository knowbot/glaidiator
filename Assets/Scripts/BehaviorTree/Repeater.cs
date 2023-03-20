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
            state = NodeState.RUNNING;
            NodeState childState = state;
            
            if (_count == -1)
            {
                while (true) // FIXME: less dirty solution?
                {
                    childState = _child.Evaluate();
                }
            }
            
            if (_count > 0)
            {
                while (_count > 0)
                {
                    childState = _child.Evaluate();
                    _count--;
                }

                state = childState;
            }

            /*
            switch (_child.Evaluate())
            {
                case NodeState.FAILURE:
                    
            }
            */
            

            return state;
        }
    }
}