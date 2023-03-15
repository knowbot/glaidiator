using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{

    public class Inverter : Decorator
    {
        private Node _child;
        
        public Inverter() : base() {}

        public Inverter(List<Node> children) : base(children) {}

        public Inverter(Node child)
        {
            _child = child;
        }
        
        public override NodeState Evaluate()
        {

            switch (_child.Evaluate())
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
    }
}
