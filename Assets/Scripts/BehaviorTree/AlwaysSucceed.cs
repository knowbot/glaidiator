using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{

    public class AlwaysSucceed : Decorator
    {

        public AlwaysSucceed() : base() { }
        
        public AlwaysSucceed(List<Node> children) : base(children) {}

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
            Node clone;
            if (_child != null)
            {
                clone = new AlwaysSucceed(_child.Clone());
            }
            else
            {
                clone = new AlwaysSucceed();
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
            return new AlwaysSucceed(EvolutionManager.GetNewRandomNode().Randomized());
        }
    }
    
    
}