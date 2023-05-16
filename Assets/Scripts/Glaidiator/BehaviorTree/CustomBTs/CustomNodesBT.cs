using System;
using System.Collections.Generic;
using BehaviorTree;

namespace BasicAI
{
    public class CustomNodesBT : BTree
    {
        private static float aggroDist = 6f;
        private static float atkDist = 2f;
        
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
                        new TaskWait(),
                        new TaskMoveForward(),                                    
                    })
                }),
                // default patrol behaviour
                new Selector(new List<Node>
                {
                    new Sequence(new List<Node> {
                        new Inverter(new CheckHasWP()),
                        new TaskSetWP(4f)
                    }),
                    new Sequence(new List<Node>
                    {
                        new CheckHasWP(),
                        new CheckWPDistance(0.01f),
                        new TaskClearWP(),
                        new TaskTurnRight(),
                        new TaskSetWP(4f)
                    }),
                    new Sequence(new List<Node>
                    {
                        new TaskMoveForward()
                    })
                })
                
                
            });
            
            root.SetOwner(_ownerChar);
            root.SetTree(this);
            SetData("enemy", _enemyChar);
            return root;
        }

        public override BTree Clone()
        {
            throw new NotImplementedException();
        }
    }
}