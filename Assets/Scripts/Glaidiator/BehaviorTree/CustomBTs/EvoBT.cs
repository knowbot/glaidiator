using BehaviorTree;
using Glaidiator.Model;

namespace BasicAI
{
    public class EvoBT : BTree
    {
        public EvoBT(Character owner) : base(owner) { }

        public EvoBT(Node root) : base(root) { }
        
        public EvoBT() : base() { }

        protected override Node SetupTree()
        {
            return root;
        }

        public override BTree Clone()
        {
            BTree newTree = new EvoBT();
            newTree.SetRoot(root.Clone());
            return newTree;
        }
    }
}