using System;
using System.Collections.Generic;
using Glaidiator.Utils;
using Random = UnityEngine.Random;

namespace Glaidiator.BehaviorTree.Base
{
    [Serializable]
    public class Selector : Composite
    {
        public Selector() : base() {}

        public Selector(List<Node> children) : base(children) { }

        public Selector(BTree btree, List<Node> children) : base(btree, children) {}

        public override NodeState Evaluate()
        {
            foreach (Node node in Children)
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
            var clone = new Selector();
            clone.SetTree(tree);
            foreach (Node child in Children)
            {
                clone.Attach(child.Clone());
            }

            return clone;
        }

        // returns a new selector-node with new random children
        public override Node Randomized()
        {
            int newCount = Random.Range(2, EvoManager.MaxChildren);
            List<Node> newChildren = new List<Node>();
            
            for (int i = 0; i < newCount; i++)
            {
                newChildren.Add(BTreeFactory.GetRandomNode().Randomized());
            }

            return new Selector(newChildren);
        }
    }
}