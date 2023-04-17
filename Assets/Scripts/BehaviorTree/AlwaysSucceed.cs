using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{

    public class AlwaysSucceed : Decorator
    {
        
        public AlwaysSucceed(Node child)
        {
            _child = child;
        }

        public override NodeState Evaluate()
        {
            //_child.Evaluate();

            switch (_child.Evaluate())
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
            Node clone = new AlwaysSucceed(_child.Clone());
            return clone;
        }
        
    }
    
    
}