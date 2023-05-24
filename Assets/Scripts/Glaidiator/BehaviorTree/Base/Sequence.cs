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
            tree.currentNode = this;
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
            var clone = new Sequence();
            clone.SetTree(tree);
            foreach (Node child in Children)
            {
                clone.Attach(child.Clone());
            }
            return clone;
        }

        public override void Mutate()
        {
            int choice = Random.Range(0, 3);
            switch (choice)
            {
                // remove random child
                case 0:
                    Children.RemoveAt(Random.Range(0, Children.Count)); // use detach?
                    //Detach(children[Random.Range(0, children.Count)]);
                    break;
                // add new random child
                case 1:
                    Attach(EvoManager.Instance.GetRandomNode().Clone());
                    break;
                // shuffle order of children
                case 2:
                    Children.Shuffle();
                    break;
            }
        }

        // returns a new sequence-node with 1-5 new random children
        public override Node Randomized()
        {
            int newCount = Random.Range(2, EvoManager.MaxChildren);
            List<Node> newChildren = new List<Node>();
            
            for (int i = 0; i < newCount; i++)
            {
                newChildren.Add(EvoManager.Instance.GetRandomNode().Randomized());
            }

            return new Sequence(newChildren);
        }
    }
}