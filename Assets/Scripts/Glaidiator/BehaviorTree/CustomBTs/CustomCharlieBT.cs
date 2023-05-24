using System;
using System.Collections.Generic;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.BehaviorTree.CustomNodes;
using Glaidiator.BehaviorTree.CustomNodes.CheckNodes;
using Glaidiator.BehaviorTree.CustomNodes.TaskNodes;
using Glaidiator.BehaviorTree.LeafNodes.ConditionNodes;

namespace Glaidiator.BehaviorTree.CustomBTs
{
    public class CustomCharlieBT : BTree
    {
        private float aggroDist = 10f;
        private float rangedDist = 8f;
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
                        //new CheckEnemyDistance(meleeDist),
                        new Selector(new List<Node> // if enemy melee
                        {
                            new CheckEnemyAction("Light Attack"),
                            new CheckEnemyAction("Heavy Attack"),
                        }),
                        
                        new Selector(new List<Node>
                        {
                            new Sequence(new List<Node> // try block or
                            {
                                new CheckEnemyDistance(meleeDist),
                                new CheckCanDoAction("block"),
                                new TaskFaceEnemy(),
                                new TaskBlock(),
                            }),
                            new Sequence(new List<Node> // try dodge
                            {
                                new CheckEnemyDistance(meleeDist),
                                new CheckOwnStamina(40f),
                                new CheckCanDoAction("dodge"),
                                new TaskBackEnemy(),
                                new TaskMoveForward(),
                                new TaskDodge(),
                            }),
                            new Sequence(new List<Node> // try move away
                            {
                                new CheckEnemyDistance(meleeDist*2),
                                new TaskBackEnemy(),
                                new Selector(new List<Node>
                                {
                                    new CheckArenaBounds(1f),
                                    // keep direction if no wall
                                    new Sequence(new List<Node> // if wall try turn left
                                    {
                                        new TaskTurnLeft(),
                                        new CheckArenaBounds(1f),
                                        
                                    }),
                                    new Sequence(new List<Node> // if wall try turn right
                                    {
                                        new TaskTurnRight(),
                                        new CheckArenaBounds(1f),
                                    }),
                                }),
                                new TaskMoveForward(),
                            }),
                        }),
                    }),
                    
                    // defend ranged sequence
                    new Sequence(new List<Node>
                    {
                        new CheckEnemyAction("Ranged Attack"), // if enemy ranged
                        new Selector(new List<Node>
                        {
                            new Sequence(new List<Node> // try block or
                            {
                                new CheckCanDoAction("block"),
                                new TaskFaceEnemy(),
                                new TaskBlock(),
                            }),
                            new Sequence(new List<Node> // try dodge
                            {
                                new CheckOwnStamina(60f),
                                new CheckCanDoAction("dodge"),
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
                        new CheckEnemyDistance(meleeDist), 
                        new CheckCanDoAction("atkLight"),
                        new TaskFaceEnemy(),
                        new TaskLightAtk(),
                    }),
                    new Sequence(new List<Node> // try dodge into heavyAtk
                    {
                        new CheckOwnStamina(50f),
                        new CheckCanDoAction("dodge"),
                        new CheckCanDoAction("atkHeavy"), // might not have stam for both
                        new Inverter(new CheckEnemyDistance(3f)),
                        new CheckEnemyDistance(5f),
                        new TaskFaceEnemy(),
                        new TaskDodge(), // gap close if (3 < dist < 5)
                        new TaskWait(),
                        new CheckEnemyDistance(meleeDist),
                        new TaskHeavyAtk(),
                    }),
                    new Sequence(new List<Node>
                    {
                        new CheckCanDoAction("atkRanged"),
                        new CheckEnemyDistance(rangedDist),
                        new TaskFaceEnemy(),
                        new CheckRangedDirection(30f), // aim good?
                        new TaskRangedAtk(),
                    }),
                }),

                // select movement branch
                new Selector(new List<Node>
                {
                    // evade sequence
                    new Sequence(new List<Node> // evade if less hp than enemy
                    {
                        new Inverter(new CheckCompareHealth(0f)), 
                        new CheckEnemyDistance(rangedDist),
                        new Sequence(new List<Node>// run away until threshold distance
                        {
                            new TaskBackEnemy(),
                            new TaskMoveForward(),
                            new TaskStop(),
                            new CheckArenaBounds(1f),
                            new TaskMoveForward(),
                        }),
                        
                        new Inverter(new CheckArenaBounds(1f)),
                        new Selector(new List<Node> // turn if hitting wall
                        {
                            new Sequence(new List<Node>
                            {
                                new TaskStop(),
                                new TaskTurnRight(),
                                new TaskMoveForward(),
                                new CheckArenaBounds(1f),
                            }),
                            new Sequence(new List<Node>
                            {
                                new TaskStop(),
                                new TaskTurnLeft(),
                                new TaskMoveForward(),
                                new CheckArenaBounds(1f),
                            }),
                        }),
                    }),
                    // move to melee sequence
                    new Sequence(new List<Node> // stop when melee dist
                    { 
                        new CheckEnemyDistance(meleeDist), // if less
                        new CheckOwnHealth(evadeThresholdHP),
                        new CheckOwnStamina(evadeThresholdStam),
                        new TaskFaceEnemy(),
                        new TaskStop(),
                    }),
                    new Sequence(new List<Node> // move until melee dist
                    {
                        new Inverter(new CheckEnemyDistance(meleeDist)), // if more
                        new CheckOwnHealth(evadeThresholdHP),
                        new CheckOwnStamina(evadeThresholdStam),
                        new TaskFaceEnemy(),
                        new TaskMoveForward(),
                    }),
                }),
            });
            
            root.SetTree(this);
            SetData("enemy", Enemy);
            return root;
        }

        public override BTree Clone()
        {
            throw new NotImplementedException();
        }
    }
}