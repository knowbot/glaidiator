using System;
using System.Collections.Generic;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.BehaviorTree.LeafNodes.ConditionNodes;
using Glaidiator.BehaviorTree.LeafNodes.TaskNodes;

namespace Glaidiator.BehaviorTree.CustomBTs
{
    public class CustomBobBT : BTree
    {
        private float aggroDist = 8f;
        private float rangedDist = 6f;
        private float meleeDist = 2f;
        private float evadeThresholdHP = 30f;
        private float evadeThresholdStam = 20f;
        
        protected override Node SetupTree()
        {
            Node root = new Selector(new List<Node>
            {
                // select defense branch
                new Selector(new List<Node>
                {
                    // defend melee sequence
                    new Sequence(new List<Node>
                    {
                        new ConditionEnemyDistance(meleeDist),
                        new Selector(new List<Node> // if enemy melee
                        {
                            new ConditionEnemyAction("Light Attack"),
                            new ConditionEnemyAction("Heavy Attack"),
                        }),
                        
                        new Selector(new List<Node>
                        {
                            new Sequence(new List<Node> // try block or
                            {
                                new ConditionCanDoAction("block"),
                                new TaskFaceEnemy(),
                                new TaskBlock(),
                            }),
                            new Sequence(new List<Node> // try dodge
                            {
                                new ConditionCanDoAction("dodge"),
                                new TaskBackEnemy(),
                                new TaskMoveForward(),
                                new TaskDodge(),
                            })
                        }),
                    }),
                    
                    // defend ranged sequence
                    new Sequence(new List<Node>
                    {
                        new ConditionEnemyAction("Ranged Attack"), // if enemy ranged
                        new Selector(new List<Node>
                        {
                            new Sequence(new List<Node> // try block or
                            {
                                new ConditionCanDoAction("block"),
                                new TaskFaceEnemy(),
                                new TaskBlock(),
                            }),
                            new Sequence(new List<Node> // try dodge
                            {
                                new ConditionCanDoAction("dodge"),
                                new TaskBackEnemy(),
                                new TaskTurnLeft(),
                                new TaskMoveForward(),
                                new TaskWait(),
                                new TaskDodge(),
                            }),
                        }),
                    }),
                }),
                
                // select offense branch
                new Selector(new List<Node>
                {
                    new Sequence(new List<Node> // try melee lightAtk
                    { 
                        new ConditionEnemyDistance(meleeDist), 
                        new ConditionCanDoAction("atkLight"),
                        new TaskFaceEnemy(),
                        new TaskLightAtk(),
                    }),
                    new Sequence(new List<Node> // try dodge into heavyAtk
                    {
                        new ConditionCanDoAction("dodge"),
                        new ConditionCanDoAction("atkHeavy"), // might not have stam for both
                        new Inverter(new ConditionEnemyDistance(3f)),
                        new ConditionEnemyDistance(5f),
                        new TaskFaceEnemy(),
                        new TaskDodge(), // gap close if (3 < dist < 5)
                        new TaskWait(),
                        new ConditionEnemyDistance(meleeDist),
                        new TaskHeavyAtk(),
                    }),
                }),
                
                // select movement branch
                new Selector(new List<Node>
                {
                    // evade sequence
                    new Sequence(new List<Node> // evade if less hp than enemy
                    {
                        new Inverter(new ConditionCompareHealth(1f)), 
                        new ConditionEnemyDistance(rangedDist),
                        new Sequence(new List<Node>// run away until threshold distance
                        {
                            new TaskBackEnemy(),
                            new TaskMoveForward(),
                            new TaskStop(),
                            new ConditionArenaBounds(1f),
                            new TaskMoveForward(),
                        }),
                        
                        new Inverter(new ConditionArenaBounds(1f)),
                        new Selector(new List<Node> // turn if hitting wall
                        {
                            new Sequence(new List<Node>
                            {
                                new TaskStop(),
                                new TaskTurnRight(),
                                new TaskMoveForward(),
                                new ConditionArenaBounds(1f),
                            }),
                            new Sequence(new List<Node>
                            {
                                new TaskStop(),
                                new TaskTurnLeft(),
                                new TaskMoveForward(),
                                new ConditionArenaBounds(1f),
                            }),
                        }),
                    }),
                    // move to melee sequence
                    new Sequence(new List<Node> // stop when melee dist
                    { 
                        new ConditionEnemyDistance(meleeDist), // if less
                        new ConditionOwnHealth(evadeThresholdHP),
                        new ConditionOwnStamina(evadeThresholdStam),
                        new TaskFaceEnemy(),
                        new TaskStop(),
                    }),
                    new Sequence(new List<Node> // move until melee dist
                    {
                        new Inverter(new ConditionEnemyDistance(meleeDist)), // if more
                        new ConditionOwnHealth(evadeThresholdHP),
                        new ConditionOwnStamina(evadeThresholdStam),
                        new TaskFaceEnemy(),
                        new TaskMoveForward(),
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
            throw new NotImplementedException();
        }
    }
}