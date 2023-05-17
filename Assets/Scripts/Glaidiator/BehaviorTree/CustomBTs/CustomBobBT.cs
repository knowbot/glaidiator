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
                // select defense branch
                new Selector(new List<Node>
                {
                    // block melee sequence
                    new Sequence(new List<Node>
                    {
                        new CheckCanDoAction("block"),
                        new CheckEnemyDistance(meleeDist),
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyAction("Light Attack"),
                            new TaskFaceEnemy(),
                            new TaskBlock(),
                        }),
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyAction("Heavy Attack"),
                            new TaskFaceEnemy(),
                            new TaskBlock(),
                        }),
                        
                    }),
                    
                    // block ranged sequence
                    new Sequence(new List<Node>
                    {
                        new CheckCanDoAction("block"),
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyAction("Ranged Attack"),
                            new TaskFaceEnemy(),
                            new TaskBlock(),
                        }),
                    }),
                    
                    // dodge sequence
                    new Sequence(new List<Node>
                    {
                        new CheckCanDoAction("block"),
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyAction("Ranged Attack"),
                            new TaskFaceEnemy(),
                            new TaskBlock(),
                        }),
                    }),
                }),
                
                // select movement branch
                new Selector(new List<Node>
                {
                    new Sequence(new List<Node>
                    { 
                        new CheckEnemyDistance(meleeDist),
                        new TaskFaceEnemy(),
                        new TaskStop(),
                    }),
                    
                    new Sequence(new List<Node>
                    {
                        new Inverter(new CheckEnemyDistance(meleeDist)),
                        new TaskFaceEnemy(),
                        new TaskMoveForward(),
                    }),
                }),
                
                // select offense branch
                new Selector(new List<Node>
                {
                    new Sequence(new List<Node>
                    { 
                        new CheckCanDoAction("atkLight"),
                        new TaskFaceEnemy(),
                        new TaskLightAtk(),
                    }),
                    
                    new Sequence(new List<Node>
                    {
                        new CheckCanDoAction("dodge"),
                        new CheckCanDoAction("atkHeavy"),
                        new Inverter(new CheckEnemyDistance(3f)),
                        
                    }),
                    
                }),
                
            });
            
            
            root.SetOwner(owner);
            root.SetTree(this);
            SetData("enemy", enemy);
            return root;
        }

        public override BTree Clone()
        {
            throw new System.NotImplementedException();
        }
    }
}