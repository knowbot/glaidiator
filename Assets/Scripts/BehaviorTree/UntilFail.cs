using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviorTree
{
    public class UntilFail : Decorator
    {

        public UntilFail() : base() { }
        public UntilFail(List<Node> children) : base(children) {}
        public UntilFail(Node child) : base(child)
        {
            _child = child;
        }

        /**
         * reprocess/loop child node until child evaluates as failure,
         * then return success to break loop.
         */
        public override NodeState Evaluate()
        {
            // this node will be success when child fails
            while (state != NodeState.SUCCESS) 
            {
                switch (_child.Evaluate())
                {
                    case NodeState.FAILURE:
                        state = NodeState.SUCCESS;
                        return state;
                    case NodeState.SUCCESS:
                        state = NodeState.FAILURE;
                        continue;
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        continue;
                }
            }

            return state;
        }

        public override Node Clone()
        {
            Node clone;
            if (_child != null)
            {
                clone = new UntilFail(_child.Clone());
            }
            else
            {
                clone = new UntilFail();
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
            return new UntilFail(EvolutionManager.GetNewRandomNode().Randomized());
        }
    }
}