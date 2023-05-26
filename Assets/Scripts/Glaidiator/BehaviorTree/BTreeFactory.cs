using System.Collections.Generic;
using System.Linq;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.BehaviorTree.CustomNodes;
using Glaidiator.BehaviorTree.CustomNodes.CheckNodes;
using Glaidiator.BehaviorTree.CustomNodes.TaskNodes;
using Glaidiator.BehaviorTree.LeafNodes.ConditionNodes;
using Glaidiator.Utils;
using UnityEngine;

namespace Glaidiator.BehaviorTree
{ 
    public static class BTreeFactory
    { 
        private static List<Node> _prototypes; // collection of behaviors to sample from
        private static Dictionary<string, Node> _prototypesMap;
        private static List<Composite> _roots;

        static BTreeFactory()
        {
            InitPrototypes();
            InitRoots();
        }

        #region Tree Creation

        public static BTree CreateRandom()
        {
            return new BTree(GetRandomRoot().Randomized());
        }
        
        public static BTree CreateVariant(BTree tree, float variantChance)
        {
            BTree variant = tree.Clone();
                variant.Mutate();
                List<Node> nodes = new();
                variant.Root.Flatten(nodes);
                foreach (Node node in nodes.Where(node => MathStuff.Rand.NextFloat() < variantChance)) node.Mutate();
                return variant;
        }
        
        public static BTree CreateAshley()
        {
            const float aggroDist = 45f;
            const float rangedDist = 6f;
            const float meleeDist = 2f;
            return new BTree(new Selector(new List<Node>
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
            }));
        }
        public static BTree CreateBob()
        {
            const float rangedDist = 6f;
            const float meleeDist = 2f;
            const float evadeThresholdHealth = 30f;
            const float evadeThresholdStamina = 20f;
            return new BTree(new Selector(new List<Node>
            {
                // select defense branch
                new Selector(new List<Node>
                {
                    // defend melee sequence
                    new Sequence(new List<Node>
                    {
                        new CheckEnemyDistance(meleeDist),
                        new Selector(new List<Node> // if enemy melee
                        {
                            new CheckEnemyAction("atkLight"),
                            new CheckEnemyAction("atkHeavy"),
                        }),
                        
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
                                new CheckCanDoAction("dodge"),
                                new TaskBackEnemy(),
                                new TaskMoveForward(),
                                new TaskDodge(),
                            })
                        }),
                    }),
                    
                    // defend ranged sequence
                    new Sequence(new List<Node>
                    {
                        new CheckEnemyAction("atkRanged"), // if enemy ranged
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
                        new CheckOwnHealth(evadeThresholdHealth),
                        new CheckOwnStamina(evadeThresholdStamina),
                        new TaskFaceEnemy(),
                        new TaskStop(),
                    }),
                    new Sequence(new List<Node> // move until melee dist
                    {
                        new Inverter(new CheckEnemyDistance(meleeDist)), // if more
                        new CheckOwnHealth(evadeThresholdHealth),
                        new CheckOwnStamina(evadeThresholdStamina),
                        new TaskFaceEnemy(),
                        new TaskMoveForward(),
                    }),
                }),
            }));
        }
        public static BTree CreateCharlie()
        {
            const float rangedDist = 8f;
            const float meleeDist = 2f;
            const float evadeThresholdHealth = 30f;
            const float evadeThresholdStamina = 20f;
            
            return new BTree(new Selector(new List<Node>
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
                        new CheckOwnHealth(evadeThresholdHealth),
                        new CheckOwnStamina(evadeThresholdStamina),
                        new TaskFaceEnemy(),
                        new TaskStop(),
                    }),
                    new Sequence(new List<Node> // move until melee dist
                    {
                        new Inverter(new CheckEnemyDistance(meleeDist)), // if more
                        new CheckOwnHealth(evadeThresholdHealth),
                        new CheckOwnStamina(evadeThresholdStamina),
                        new TaskFaceEnemy(),
                        new TaskMoveForward(),
                    }),
                }),
            }));
        }

        #endregion
       

        #region Node Methods & Initializers

         private static Node GetRandomRoot()
        {
            return _roots[Random.Range(0, _roots.Count)].Clone();
        }
         
        private static Node GetRandomPrototype()
        { 
            return _prototypes[Random.Range(0, _prototypes.Count)].Clone();
        }

        public static Node GetRandomNode()
        {
            float p = MathStuff.Rand.NextFloat();
            return p > 0.25 ? GetRandomPrototype() : GetRandomRoot();
        }

        private static void InitRoots()
        {
            _roots = new List<Composite>
            {
                new Selector(),
                new Sequence()
            };
        }
        
         // init prototype samples for random mutations
        private static void InitPrototypes() // TODO: Include decorator nodes, especially prototypes with Inverter node
        {
            _prototypesMap = new Dictionary<string, Node>();
            // prototypesMap.Add("TaskLightAtk", new TaskLightAtk());
            // prototypesMap.Add("TaskHeavyAtk", new TaskHeavyAtk());
            // prototypesMap.Add("TaskRangedAtk", new TaskRangedAtk());
            // prototypesMap.Add("TaskBlock", new TaskBlock());
            // prototypesMap.Add("TaskDodge", new TaskDodge());
            // prototypesMap.Add("CheckLightAtk", new CheckCanDoAction("atkLight"));
            // prototypesMap.Add("CheckHeavyAtk", new CheckCanDoAction("atkHeavy"));
            // prototypesMap.Add("CheckRangedAtk", new CheckCanDoAction("atkRanged"));
            // prototypesMap.Add("CheckBlock", new CheckCanDoAction("block"));
            // prototypesMap.Add("CheckDodge", new CheckCanDoAction("dodge"));
            
            _prototypesMap.Add("TaskFaceEnemy", new TaskFaceEnemy());
            _prototypesMap.Add("TaskBackEnemy", new TaskBackEnemy());
            _prototypesMap.Add("TaskMoveForward", new TaskMoveForward());
            _prototypesMap.Add("TaskTurnLeft", new TaskTurnLeft());
            _prototypesMap.Add("TaskTurnRight", new TaskTurnRight());
            _prototypesMap.Add("CheckArenaBounds", new CheckArenaBounds(1f));
            
            _prototypesMap.Add("CheckEnemyDistanceMelee", new CheckEnemyDistance(2f));
            _prototypesMap.Add("CheckEnemyDistanceRanged", new CheckEnemyDistance(6f));
            _prototypesMap.Add("CheckOwnHealth", new CheckOwnHealth(50f));
            _prototypesMap.Add("CheckOwnStamina", new CheckOwnStamina(50f));
            _prototypesMap.Add("CheckEnemyHealth", new CheckEnemyHealth(50f));
            _prototypesMap.Add("CheckEnemyStamina", new CheckEnemyStamina(50f));
            // prototypesMap.Add("TaskSetWP", new TaskSetWP(2f));
            // prototypesMap.Add("TaskClearWP", new TaskClearWP());
            // prototypesMap.Add("CheckHasWP", new CheckHasTarget("wp"));
            // prototypesMap.Add("CheckWPDistance", new CheckTargetDistance("wp", 0.01f));
            _prototypesMap.Add("TaskWait", new TaskWait());

            _prototypesMap.Add("CheckEnemyLight", new CheckEnemyAction("atkLight"));
            _prototypesMap.Add("CheckEnemyHeavy", new CheckEnemyAction("atkHeavy"));
            _prototypesMap.Add("CheckEnemyRanged", new CheckEnemyAction("atkRanged"));
            _prototypesMap.Add("CheckEnemyBlock", new CheckEnemyAction("block"));
            _prototypesMap.Add("CheckEnemyDodge", new CheckEnemyAction("dodge"));
            _prototypesMap.Add("ModuleAtkLight", 
                new Module(
                    "ModuleAtkLight", 
                    new Sequence(new List<Node> 
                    {
                        new CheckCanDoAction("atkLight"),
                        new TaskFaceEnemy(),
                        new TaskLightAtk(),
                    })
                )
            );
            
            _prototypesMap.Add("ModuleAtkHeavy", 
                new Module(
                    "ModuleAtkHeavy", 
                    new Sequence(new List<Node> 
                    {
                        new CheckCanDoAction("atkHeavy"),
                        new TaskFaceEnemy(),
                        new TaskHeavyAtk(),
                    })
                )
            );
            
            _prototypesMap.Add("ModuleAtkRanged",
                new Module(
                    "ModuleAtkRanged",
                    new Sequence(new List<Node>
                    {
                        new Inverter(new CheckEnemyDistance(3f)),
                        new CheckCanDoAction("atkRanged"),
                        new CheckEnemyDistance(6f),
                        new TaskFaceEnemy(),
                        new CheckRangedDirection(30f),
                        new TaskRangedAtk(),
                    })
                )
            );
            
            _prototypesMap.Add("ModuleBlock", 
                new Module(
                    "ModuleBlock", 
                    new Sequence(new List<Node> 
                    {
                        new CheckCanDoAction("block"),
                        new TaskFaceEnemy(),
                        new TaskBlock(),
                    })
                )
            );
            
            _prototypesMap.Add("ModuleDodge", 
                new Module(
                    "ModuleDodge", 
                    new Sequence(new List<Node> 
                    {
                        new CheckCanDoAction("dodge"),
                        new TaskDodge(),
                    })
                )
            );


            _prototypes = _prototypesMap.Values.ToList();
        }

        #endregion
    }
}