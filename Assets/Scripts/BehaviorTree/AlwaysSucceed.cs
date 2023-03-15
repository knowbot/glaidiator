using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{

    public class AlwaysSucceed : Decorator
    {
        private Node _child;
        
        public AlwaysSucceed(Node child)
        {
            _child = child;
        }

        public override NodeState Evaluate()
        {
            _child.Evaluate();
            state = NodeState.SUCCESS;
            return state;
        }
    }
}