using System.Collections.Generic;
using BehaviorTree;
using Glaidiator.Model;

namespace BasicAI
{
    
    public class xBossBT : BTree // deprecated
    {

        public static float aggroRange = 3f;
        public static float lightAtkRange = 1.5f;

        protected override Node SetupTree()
        {
            
            Node root = new Selector(this, new List<Node>
            {
                new Sequence(this, new List<Node>
                {
                    new xCheckEnemyInRange(this, GetOwnerChar()),
                    new xTaskGoToTarget(this, GetOwnerChar()),
                    new xTaskAttack(this, GetOwnerChar())
                }),
                new xTaskPatrol(this, GetOwnerChar()),
            });
            return root;
        }

        public override BTree Clone()
        {
            BTree newTree = new xBossBT(_ownerChar);
            newTree.SetRoot(_root.Clone());
            return newTree;
        }

        //public BossBT(Transform transform) : base(transform)
        //{
        //}

        public xBossBT(Character character) : base(character)
        {
        }
        
        public xBossBT(Node root) : base(root) {}

        public xBossBT() : base()
        {
            
        }
    }
}