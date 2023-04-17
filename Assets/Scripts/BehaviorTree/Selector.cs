using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviorTree
{
    public class Selector : Node
    {
        public Selector() : base() {}
        public Selector(List<Node> children) : base(children) {}

        public Selector(BTree btree, List<Node> children) : base(btree, children) {}

        public override NodeState Evaluate()
        {
            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        continue;
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        return state;
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                    default:
                        continue;
                }
            }

            state = NodeState.FAILURE;
            return state;
        }

        public override Node Clone()
        {
            Node clone = new Selector();
            clone.SetParent(parent);
            clone.SetTree(tree);
            foreach (Node child in children)
            {
                clone.Attach(child.Clone());
            }

            return clone;
        }
    }
}