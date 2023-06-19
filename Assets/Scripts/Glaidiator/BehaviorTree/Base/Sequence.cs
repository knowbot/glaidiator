using System.Collections.Generic;
using Glaidiator.Utils;
using UnityEngine;

namespace Glaidiator.BehaviorTree.Base
{
    public class Sequence : Composite
    {
        public Sequence() : base() { }

        public Sequence(List<Node> children) : base(children) { }
        
        public Sequence(BTree btree, List<Node> children) : base(btree, children) {}

        public override NodeState Evaluate()
        {
            tree.Active = this;
            bool hasChildRunning = false;
            
            foreach (Node node in Children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        hasChildRunning = true;
                        return state;
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
            var clone = new Sequence();
            clone.SetTree(tree);
            foreach (Node child in Children)
            {
                clone.Attach(child.Clone());
            }
            return clone;
        }

        // returns a new sequence-node with new random children
        public override Node Randomized()
        {
            int newCount = Random.Range(2, EvoManager.MaxChildren);
            List<Node> newChildren = new List<Node>();
            
            for (int i = 0; i < newCount; i++)
            {
                newChildren.Add(BTreeFactory.GetRandomNode().Randomized());
            }

            return new Sequence(newChildren);
        }
    }
}