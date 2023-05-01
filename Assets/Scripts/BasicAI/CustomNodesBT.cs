using System.Collections.Generic;
using BehaviorTree;

namespace BasicAI
{
    public class CustomNodesBT : BTree
    {
        private static float aggroDist = 4f;
        private static float atkDist = 1f;
        
        public CustomNodesBT() : base()
        {
            
        }
            
        protected override Node SetupTree()
        {
            Node root = new Selector(new List<Node>
            {
                // approach and attack behaviour
                new Selector(new List<Node>
                {
                    new Sequence(new List<Node>
                    {
                        new CheckEnemyDistance(atkDist),
                        new TaskFaceEnemy(),
                        new TaskLightAtk(),
                    }),
                    new Sequence(new List<Node>
                    {
                        new CheckEnemyDistance(aggroDist),
                        new TaskFaceEnemy(),
                        new TaskMoveForward(),                                    
                    })
                }),
                new TaskPatrol(this, GetOwnerChar()),
                
                // default patrol behaviour
                /*
                new Selector(new List<Node>
                {
                    new Sequence(new List<Node>
                    {
                        ()
                    }),
                    new Sequence(new List<Node>
                    {
                        ()
                    })
                })
                */
            });
            
            root.SetOwner(_ownerChar);
            root.SetTree(this);
            SetData("enemy", _enemyChar.Movement);
            return root;
        }

        public override BTree Clone()
        {
            throw new System.NotImplementedException();
        }
    }
}