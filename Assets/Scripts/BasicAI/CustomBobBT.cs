using System.Collections.Generic;
using BehaviorTree;

namespace BasicAI
{
    public class CustomBobBT : BTree
    {
        private float aggroDist = 8f;
        private float rangedDist = 6f;
        private float meleeDist = 2f;

        
        protected override Node SetupTree()
        {
            Node root = new Selector(new List<Node>
            {

            });
            
            
            root.SetOwner(_ownerChar);
            root.SetTree(this);
            SetData("enemy", _enemyChar);
            return root;
        }

        public override BTree Clone()
        {
            throw new System.NotImplementedException();
        }
    }
}