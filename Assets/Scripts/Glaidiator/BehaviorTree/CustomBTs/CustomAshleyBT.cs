using System;
using System.Collections.Generic;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.BehaviorTree.CustomNodes;
using Glaidiator.BehaviorTree.CustomNodes.CheckNodes;
using Glaidiator.BehaviorTree.CustomNodes.TaskNodes;
using Glaidiator.BehaviorTree.LeafNodes.ConditionNodes;

namespace Glaidiator.BehaviorTree.CustomBTs
{
    public class CustomAshleyBT : BTree
    {
        
        private static float aggroDist = 45f;
        private static float rangedDist = 6f;
        private static float meleeDist = 2f;
        

        public CustomAshleyBT() { }

        
        protected override Node SetupTree()
        {
            Node newRoot = new Selector(new List<Node>
            {
                // attack sequence
                new Sequence(new List<Node>{ 
                    // conditions for aggression
                    new CheckEnemyDistance(aggroDist),
                    // new CheckOwnHealth(50f), 
                    // new CheckOwnStamina(15f),
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
                        new Selector(new List<Node>
                        {
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
                            })
                        }),
                        new Sequence(new List<Node>
                        {
                            new TaskFaceEnemy(),
                            new TaskMoveForward(),
                            new TaskWait(),
                            new TaskStop(),
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
                        new Sequence(new List<Node>
                        {
                            new Inverter(new CheckEnemyDistance(4f)),
                            new TaskFaceEnemy(),
                            new TaskMoveForward(),
                            new TaskWait(),
                            new TaskStop(),
                        }),
                        // run away until threshold distance
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyDistance(4f),
                            new TaskBackEnemy(),
                            new TaskMoveForward(),
                            new TaskStop(),
                            new CheckArenaBounds(1f),
                            new TaskMoveForward(),
                        }),
                        // turn if hitting wall
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyDistance(3f),
                            new Inverter(new CheckArenaBounds(1f)),
                            new TaskStop(),
                            new TaskTurnRight(),
                            new TaskMoveForward(),
                            new CheckArenaBounds(1f),
                        }),
                        new Sequence(new List<Node>
                        {
                            new CheckEnemyDistance(3f),
                            new Inverter(new CheckArenaBounds(1f)),
                            new TaskStop(),
                            new TaskTurnLeft(),
                            new TaskMoveForward(),
                            new CheckArenaBounds(1f),
                        })
                    })
                }),
                
                //new Selector(),
                
                //new Sequence(),
            });
            newRoot.SetTree(this);
            SetData("enemy", Enemy);
            return newRoot;
        }

        public override BTree Clone()
        {
            throw new NotImplementedException();
        }
    }
}