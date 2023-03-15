using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Repeater : Decorator
    {
        private Node _child;
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
            state = _child.Evaluate();
            
            
            
            if (_count == -1)
            {
                while (true)
                {
                    
                }
            }

            return state;
        }
    }
}