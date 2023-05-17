using System;
using System.Collections.Generic;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.BehaviorTree.LeafNodes.ConditionNodes;
using Glaidiator.BehaviorTree.LeafNodes.TaskNodes;

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
                    new ConditionEnemyDistance(aggroDist),
                    // new CheckOwnHealth(50f), 
                    // new CheckOwnStamina(15f),
                    new Selector(new List<Node> 
                    { 
                        new Sequence(new List<Node>
                        {
                            new Inverter(new ConditionEnemyDistance(3f)), // min range
                            new ConditionCanDoAction("atkRanged"),
                            new ConditionEnemyDistance(rangedDist),
                            new TaskFaceEnemy(),
                            new ConditionRangedDirection(30f), // aim good?
                            new TaskRangedAtk(),
                        }),
                        
                        new Randomizer(
                            new Selector(new List<Node>
                            {
                                new Sequence(new List<Node>
                                {
                                    new ConditionCanDoAction("atkHeavy"),
                                    new ConditionEnemyDistance(meleeDist),
                                    new TaskFaceEnemy(),
                                    new TaskHeavyAtk(),
                                }),
                                new Sequence(new List<Node>
                                {
                                    new ConditionCanDoAction("atkLight"),
                                    new ConditionEnemyDistance(meleeDist),
                                    new TaskFaceEnemy(),
                                    new TaskLightAtk(),
                                }),
                                new Sequence(new List<Node>
                                {
                                    new ConditionEnemyDistance(aggroDist),
                                    new Inverter(new ConditionEnemyDistance(0.5f)),
                                    new TaskFaceEnemy(),
                                    new TaskMoveForward(),
                                })
                            })
                        ),
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
                            new ConditionEnemyDistance(meleeDist),
                            new ConditionCanDoAction("block"),
                            new TaskFaceEnemy(),
                            new TaskBlock(),
                        }),
                        // dodge away if we can't block
                        new Sequence(new List<Node>
                        {
                            new ConditionEnemyDistance(meleeDist),
                            new ConditionCanDoAction("dodge"),
                            new TaskBackEnemy(),
                            new TaskDodge(),
                        }),
                        new Sequence(new List<Node>
                        {
                            new Inverter(new ConditionEnemyDistance(4f)),
                            new TaskFaceEnemy(),
                            new TaskMoveForward(),
                            new TaskWait(),
                            new TaskStop(),
                        }),
                        // run away until threshold distance
                        new Sequence(new List<Node>
                        {
                            new ConditionEnemyDistance(4f),
                            new TaskBackEnemy(),
                            new TaskMoveForward(),
                            new TaskStop(),
                            new ConditionArenaBounds(1f),
                            new TaskMoveForward(),
                        }),
                        // turn if hitting wall
                        new Sequence(new List<Node>
                        {
                            new ConditionEnemyDistance(3f),
                            new Inverter(new ConditionArenaBounds(1f)),
                            new TaskStop(),
                            new TaskTurnRight(),
                            new TaskMoveForward(),
                            new ConditionArenaBounds(1f),
                        }),
                        new Sequence(new List<Node>
                        {
                            new ConditionEnemyDistance(3f),
                            new Inverter(new ConditionArenaBounds(1f)),
                            new TaskStop(),
                            new TaskTurnLeft(),
                            new TaskMoveForward(),
                            new ConditionArenaBounds(1f),
                        })
                    })
                }),
                
                //new Selector(),
                
                //new Sequence(),
            });
            
            newRoot.SetOwner(owner);
            newRoot.SetTree(this);
            SetData("enemy", enemy);
            return newRoot;
        }

        public override BTree Clone()
        {
            throw new NotImplementedException();
        }
    }
}