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
                //new TaskPatrol(this, GetOwnerChar()),
                
                // default patrol behaviour
                new Selector(new List<Node>
                {
                    new Sequence(new List<Node>
                    {
                        new Inverter(new CheckHasWP()),
                        new TaskSetWP(2f),
                    }),
                    new Sequence(new List<Node> {
                        new CheckHasWP(),
                        new CheckDistanceToWP(0.01f),
                        new TaskTurnRight(),
                        new TaskSetWP(2f),
                    }),
                    new Sequence(new List<Node>
                    {
                        new TaskMoveForward()    
                    }),
                })
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