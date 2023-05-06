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
                    new CheckOwnHealth(50f), // condition for aggression
                    new Selector(new List<Node> 
                    { 
                        new Sequence(new List<Node>
                        {
                            new Inverter(new CheckEnemyDistance(3f)),
                            new CheckCanDoAction("Ranged Attack"),
                            new CheckEnemyDistance(rangedDist),
                            new TaskFaceEnemy(),
                            new CheckRangedDirection(), // aim good?
                            new TaskRangedAtk(),
                        }),
                        new Sequence(new List<Node>
                        {
                            new CheckCanDoAction("Heavy Attack"),
                            new CheckEnemyDistance(meleeDist),
                            new TaskFaceEnemy(),
                            new TaskHeavyAtk(),
                        }),
                        new Sequence(new List<Node>
                        {
                            new CheckCanDoAction("Light Attack"),
                            new CheckEnemyDistance(meleeDist),
                            new TaskFaceEnemy(),
                            new TaskLightAtk(),
                        })
                    }),
                }),
                // defense sequence
                new Sequence(),
                
                new Selector(),
                
                new Sequence(),
            });
            
            
            return root;
        }

        public override BTree Clone()
        {
            throw new System.NotImplementedException();
        }
    }
}