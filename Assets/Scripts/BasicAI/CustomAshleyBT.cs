using System.Collections.Generic;
using BehaviorTree;

namespace BasicAI
{
    public class CustomAshleyBT : BTree
    {
        
        private static float aggroDist = 8f;
        private static float rangedDist = 6f;
        private static float meleeDist = 2f;
        

        public CustomAshleyBT() { }

        
        protected override Node SetupTree()
        {
            Node root = new Selector(new List<Node>
            {
                // attack sequence
                new Sequence(new List<Node>{ 
                    // conditions for aggression
                    new CheckEnemyDistance(aggroDist),
                    new CheckOwnHealth(50f), 
                    new CheckOwnStamina(15f),
                    new Selector(new List<Node> 
                    { 
                        new Sequence(new List<Node>
                        {
                            new Inverter(new CheckEnemyDistance(3f)), // min range
                            new CheckCanDoAction("atkRanged"),
                            new CheckEnemyDistance(rangedDist),
                            new TaskFaceEnemy(),
                            new CheckRangedDirection(30f), // aim good?
                            new TaskRangedAtk(),
                        }),
                        new Sequence(new List<Node>
                        {
                            new CheckCanDoAction("atkHeavy"),
                            new CheckEnemyDistance(meleeDist),
                            new TaskFaceEnemy(),
                            new TaskHeavyAtk(),
                        }),
                        new Sequence(new List<Node>
                        {
                            new CheckCanDoAction("atkLight"),
                            new CheckEnemyDistance(meleeDist),
                            new TaskFaceEnemy(),
                            new TaskLightAtk(),
                        }),
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyDistance(aggroDist),
                            new Inverter(new CheckEnemyDistance(0.5f)),
                            new TaskFaceEnemy(),
                            new TaskMoveForward(),
                        }),
                        new Sequence(new List<Node>
                        {
                            new TaskFaceEnemy()
                        }),
                    }),
                }),
                
                // defense sequence
                new Sequence(new List<Node>
                {
                    // conditions for defense sequence?
                    new Selector(new List<Node>
                    {
                        // block when enemy in melee range
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyDistance(meleeDist),
                            new CheckCanDoAction("block"),
                            new TaskFaceEnemy(),
                            new TaskBlock(),
                        }),
                        // dodge away if we can't block
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyDistance(meleeDist),
                            new CheckCanDoAction("dodge"),
                            new TaskBackEnemy(),
                            new TaskDodge(),
                        }),
                        // run away until threshold distance
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyDistance(4f),
                            new TaskBackEnemy(),
                            new TaskMoveForward(),
                        }),
                        new Sequence(new List<Node>
                        {
                            new Inverter(new CheckEnemyDistance(4f)),
                            new TaskFaceEnemy(),
                        })
                        
                    })
                }),
                
                //new Selector(),
                
                //new Sequence(),
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