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
                
                new Sequence(new List<Node> {
                        new TaskMoveForward(2f),
                        new TaskTurnRight(), // 90 degree turn
                        new TaskMoveForward(2f),
                        new TaskTurnRight(),
                        new TaskMoveForward(2f),
                        new TaskTurnRight(),
                        new TaskMoveForward(2f),
                        new TaskTurnRight()
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