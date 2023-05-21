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
            tree.currentNode = this;
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

        // returns a new selector-node with 1-5 new random children
        public override Node Randomized()
        {
            int newCount = Random.Range(2, EvoManager.MaxChildren);
            List<Node> newChildren = new List<Node>();
            
            for (int i = 0; i < newCount; i++)
            {
                newChildren.Add(EvoManager.Instance.GetRandomNode().Randomized());
            }

            return new Selector(newChildren);
        }
    }
}