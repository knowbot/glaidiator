using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Sequence : Node
    {
        public Sequence() : base() { }

        public Sequence(List<Node> children) : base(children) { }
        
        public Sequence(BTree btree, List<Node> children) : base(btree, children) {}

        public override NodeState Evaluate()
        {
            bool hasChildRunning = false;
            
            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.RUNNING:
                        hasChildRunning = true;
                        continue;
                    default:
                        state = NodeState.SUCCESS;
                        return state;

                }
            }

            state = hasChildRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return state;
        }

        public override Node Clone()
        {
            Node clone = new Sequence();
            clone.SetParent(parent); // redundant?
            clone.SetTree(tree);
            foreach (Node child in children)
            {
                clone.Attach(child.Clone());
            }

            return clone;
        }

        public override void Mutate()
        {
            throw new System.NotImplementedException();
        }

        public override Node Randomized()
        {
            throw new System.NotImplementedException();
        }
    }
}