using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DTT.Utils.Extensions;
using Random = UnityEngine.Random;


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
            clone.SetParent(parent);    // FIXME: Test for correct parent and tree refs
            clone.SetTree(tree);
            foreach (Node child in children)
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
                    children.RemoveAt(Random.Range(0, children.Count)); // use detach?
                    //Detach(children[Random.Range(0, children.Count)]);
                    break;
                // add new random child
                case 1:
                    Attach(EvolutionManager.GetNewRandomNode().Clone());
                    break;
                // shuffle order of children
                case 2:
                    children.Shuffle();
                    break;
            }
        }

        // returns a new selector-node with 1-5 new random children
        public override Node Randomized()
        {
            int newCount = Random.Range(1, 5);
            List<Node> newChildren = new List<Node>();
            
            for (int i = 0; i < newCount; i++)
            {
                newChildren.Add(EvolutionManager.GetNewRandomNode().Randomized());
            }

            return new Selector(newChildren);
        }
    }
}