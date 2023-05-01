using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{

    public class Inverter : Decorator
    {
        
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

        public override Node Clone()
        {
            Node clone;
            if (_child != null)
            {
                clone = new Inverter(_child.Clone());
            }
            else
            {
                clone = new Inverter();
            }
            
            return clone;
        }

        public override void Mutate()
        {
            if (_child == null)
            {
                _child = EvolutionManager.GetNewRandomNode().Clone();
            }
        }

        public override Node Randomized()
        {
            return new Inverter(EvolutionManager.GetNewRandomNode().Randomized());
        }
    }
}
