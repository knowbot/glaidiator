using System.Collections.Generic;
using System.Linq;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.BehaviorTree.CustomNodes;
using Glaidiator.BehaviorTree.CustomNodes.ActionNodes;
using Glaidiator.BehaviorTree.CustomNodes.ConditionNodes;
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


        public static BTree CreateRandom()
        {
            return new BTree(GetRandomRoot().Randomized());
        }

        public static BTree CreateVariant(BTree tree, float variantMutateChance)
        {
            BTree variant = tree.Clone();
            variant.Mutate();
            List<Node> nodes = new();
            variant.Root.Flatten(nodes);
            foreach (Node node in nodes.Where(node => MathStuff.Rand.NextFloat() < variantMutateChance)) node.Mutate();
            return variant;
        }


        
        #region Node Methods & Initializers

        public static Node GetRandomRoot()
        {
            return _roots[Random.Range(0, _roots.Count)].Clone();
        }
         
        public static Node GetRandomPrototype()
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
            _prototypesMap.Add("Inverter", new Inverter());
            _prototypesMap.Add("Repeater", new Repeater());
            _prototypesMap.Add("AlwaysSucceed", new AlwaysSucceed());
            // /_prototypesMap.Add("UntilFail", new UntilFail());
            _prototypesMap.Add("TaskFaceEnemy", new ActionFaceEnemy());
            _prototypesMap.Add("TaskBackEnemy", new ActionBackEnemy());
            _prototypesMap.Add("TaskMoveForward", new ActionMoveForward());
            _prototypesMap.Add("TaskTurnLeft", new ActionTurnLeft());
            _prototypesMap.Add("TaskTurnRight", new ActionTurnRight());
            _prototypesMap.Add("CheckArenaBounds", new ConditionArenaBounds(1f));
            
            _prototypesMap.Add("CheckEnemyDistanceMelee", new ConditionEnemyDistance(2f));
            _prototypesMap.Add("CheckEnemyDistanceRanged", new ConditionEnemyDistance(6f));
            _prototypesMap.Add("CheckOwnHealth", new ConditionOwnHealth(50f));
            _prototypesMap.Add("CheckOwnStamina", new ConditionOwnStamina(50f));
            _prototypesMap.Add("CheckEnemyHealth", new ConditionEnemyHealth(50f));
            _prototypesMap.Add("CheckEnemyStamina", new ConditionEnemyStamina(50f));
            // prototypesMap.Add("TaskSetWP", new TaskSetWP(2f));
            // prototypesMap.Add("TaskClearWP", new TaskClearWP());
            // prototypesMap.Add("CheckHasWP", new CheckHasTarget("wp"));
            // prototypesMap.Add("CheckWPDistance", new CheckTargetDistance("wp", 0.01f));
            _prototypesMap.Add("TaskWait", new ActionWaitTicks());

            _prototypesMap.Add("CheckEnemyLight", new ConditionEnemyAction("atkLight"));
            _prototypesMap.Add("CheckEnemyHeavy", new ConditionEnemyAction("atkHeavy"));
            _prototypesMap.Add("CheckEnemyRanged", new ConditionEnemyAction("atkRanged"));
            _prototypesMap.Add("CheckEnemyBlock", new ConditionEnemyAction("block"));
            _prototypesMap.Add("CheckEnemyDodge", new ConditionEnemyAction("dodge"));
            _prototypesMap.Add("CheckEnemyCanDoLight", new ConditionEnemyCanDoAction("atkLight"));
            _prototypesMap.Add("CheckEnemyCanDoHeavy", new ConditionEnemyCanDoAction("atkHeavy"));
            _prototypesMap.Add("CheckEnemyCanDoRanged", new ConditionEnemyCanDoAction("atkRanged"));
            _prototypesMap.Add("CheckEnemyCanDoBlock", new ConditionEnemyCanDoAction("block"));
            _prototypesMap.Add("CheckEnemyCanDoDodge", new ConditionEnemyCanDoAction("dodge"));
            _prototypesMap.Add("ModuleAtkLight", 
                new Module(
                    "ModuleAtkLight", 
                    new Sequence(new List<Node> 
                    {
                        new ConditionCanDoAction("atkLight"),
                        new ActionFaceEnemy(),
                        new ActionLightAtk(),
                    })
                )
            );
            
            _prototypesMap.Add("ModuleAtkHeavy", 
                new Module(
                    "ModuleAtkHeavy", 
                    new Sequence(new List<Node> 
                    {
                        new ConditionCanDoAction("atkHeavy"),
                        new ActionFaceEnemy(),
                        new ActionHeavyAtk(),
                    })
                )
            );
            
            _prototypesMap.Add("ModuleAtkRanged",
                new Module(
                    "ModuleAtkRanged",
                    new Sequence(new List<Node>
                    {
                        new Inverter(new ConditionEnemyDistance(3f)),
                        new ConditionCanDoAction("atkRanged"),
                        new ConditionEnemyDistance(6f),
                        new ActionFaceEnemy(),
                        new ConditionRangedDirection(30f),
                        new ActionRangedAtk(),
                    })
                )
            );
            
            _prototypesMap.Add("ModuleBlock", 
                new Module(
                    "ModuleBlock", 
                    new Sequence(new List<Node> 
                    {
                        new ConditionCanDoAction("block"),
                        new ActionFaceEnemy(),
                        new ActionBlock(),
                    })
                )
            );
            
            _prototypesMap.Add("ModuleDodge", 
                new Module(
                    "ModuleDodge", 
                    new Sequence(new List<Node> 
                    {
                        new ConditionCanDoAction("dodge"),
                        new ActionDodge(),
                    })
                )
            );


            _prototypes = _prototypesMap.Values.ToList();
        }

        #endregion

        #region Custom Tree Declarations

        public static BTree CreateTreeTester()
        {
            return new BTree(new Selector(new List<Node>
            {
                new Selector(new List<Node>
                {
                    new ActionLightAtk(),
                }),
                
                
                new Sequence(new List<Node>
                {
                    new ActionHeavyAtk(),
                    new ActionWaitState(),
                    
                }),
                
                
                new Sequence(new List<Node>
                {
                    new ActionWaitTicks(30),
                    new ActionTurnLeft(),
                    new ActionWaitTicks(30),
                    new ActionLightAtk(),
                }),
                
            }));
        }

        public static BTree CreateBaselineEvo()
        {
            const float rangedMinDist = 3f;
            const float rangedMaxDist = 8f;
            const float meleeDist = 2f;
            const float wallDist = 1f;
            const float dodgeThresholdStamina = 40f;
            const float aggroThresholdStamina = 20f;
            const float defenseHealthThreshold = 30f;

            return new BTree(new Selector(new List<Node>
            {
                new ConditionEnemyState("Dead"), // success if enemy is dead
                
                // defense branch
                new Sequence(new List<Node>
                {
                    // only defend when low hp
                    new Inverter(new ConditionOwnHealth(defenseHealthThreshold)),
                    new Selector(new List<Node> // if enemy melee
                    {
                        new ConditionEnemyAction("atkLight"),
                        new ConditionEnemyAction("atkHeavy"),
                    }),
                    
                    new Selector(new List<Node>
                    {
                        new Sequence(new List<Node> // try block or
                        {
                            new ConditionEnemyDistance(meleeDist),
                            new ConditionCanDoAction("block"),
                            new ActionFaceEnemy(),
                            new ActionBlock(),
                        }),
                        new Sequence(new List<Node> // try dodge or
                        {
                            new ConditionEnemyDistance(meleeDist),
                            new ConditionCanDoAction("dodge"),
                            new ConditionOwnStamina(dodgeThresholdStamina),
                            new ActionBackEnemy(),
                            new ActionMoveForward(),
                            new ActionDodge(),
                        }),
                        /*
                        new Sequence(new List<Node> // try move away
                        {
                            new CheckEnemyDistance(meleeDist*2f),
                            new Selector(new List<Node>
                            {
                                new Sequence(new List<Node>
                                {
                                    new TaskBackEnemy(),
                                    new CheckArenaBounds(wallDist),
                                    new TaskMoveForward(),
                                }),
                                new Sequence(new List<Node>
                                { // move through enemy if wall blocks back
                                    new Inverter(new CheckArenaBounds(wallDist)),
                                    new TaskFaceEnemy(),
                                    new TaskMoveForward(),
                                }),
                            }),
                        }),
                        */
                    }),
                }),
                
                // attack branch
                new Selector(new List<Node>
                {
                    new Sequence(new List<Node>
                    {
                        new ConditionCanDoAction("atkLight"),
                        new ConditionEnemyDistance(meleeDist),
                        new ActionFaceEnemy(),
                        new ActionLightAtk(),
                    }),
                    new Sequence(new List<Node>
                    {
                        new ConditionCanDoAction("atkHeavy"),
                        new ConditionEnemyDistance(meleeDist),
                        new ActionFaceEnemy(),
                        new ActionHeavyAtk(),
                    }),
                    new Sequence(new List<Node>
                    {
                        new ConditionCanDoAction("atkRanged"),
                        new ConditionEnemyDistance(rangedMaxDist),
                        new Inverter(new ConditionEnemyDistance(rangedMinDist)),
                        new ActionFaceEnemy(),
                        new ConditionRangedDirection(),
                        new ActionRangedAtk(),
                    }),
                }),
                
                // navigation branch offensive
                new Sequence(new List<Node> // move to melee dist
                {
                    new ConditionCompareHealth(-10f),
                    //new CheckCompareStamina(-10f),
                    new Inverter(new ConditionEnemyDistance(meleeDist)),
                    new ActionFaceEnemy(),
                    new ActionMoveForward(),
                }),
                new Sequence(new List<Node> // stop when melee dist
                {
                    new ConditionCompareHealth(-10f),
                    //new CheckCompareStamina(-10f),
                    new ConditionEnemyDistance(meleeDist),
                    new ActionFaceEnemy(),
                    new ActionStop(),
                }),
                
                // navigation branch defensive
                new Sequence(new List<Node>
                {
                    new Inverter(new ConditionCompareHealth(0f)),
                    new ConditionEnemyDistance(meleeDist * 2f),
                    new Selector(new List<Node>
                    {
                        new Sequence(new List<Node>
                        {
                            new ActionBackEnemy(),
                            new ConditionArenaBounds(wallDist),
                            new ActionMoveForward(),
                        }),
                        new Sequence(new List<Node>
                        {
                            new ActionTurnLeft(),
                            new ConditionArenaBounds(wallDist),
                            new ActionMoveForward(),
                        }),
                        new Sequence(new List<Node>
                        {
                            new ActionTurnLeft(),
                            new ConditionArenaBounds(wallDist),
                            new ActionMoveForward(),
                        }),
                    }),
                }),
                
            }));
        }
        
        public static BTree CreateAshley()
        {
            const float aggroDist = 45f;
            const float rangedDist = 6f;
            const float meleeDist = 2f;
            return new BTree(new Selector(new List<Node>
            {
                new ConditionEnemyState("Dead"), // success if enemy is dead
                // attack sequence
                new Sequence(new List<Node>
                {
                    // conditions for aggression
                    //new ConditionEnemyDistance(aggroDist),
                    // new CheckOwnHealth(50f), 
                    // new CheckOwnStamina(15f),
                    new Selector(new List<Node>
                    {
                        new Sequence(new List<Node>
                        {
                            new Inverter(new ConditionEnemyDistance(3f)), // min range
                            new ConditionCanDoAction("atkRanged"),
                            new ConditionEnemyDistance(rangedDist),
                            new ActionFaceEnemy(),
                            new ConditionRangedDirection(30f), // aim good?
                            new ActionRangedAtk(),
                        }),
                        new Selector(new List<Node>
                        {
                            new Sequence(new List<Node>
                            {
                                new ConditionCanDoAction("atkHeavy"),
                                new ConditionEnemyDistance(meleeDist),
                                new ActionFaceEnemy(),
                                new ActionHeavyAtk(),
                            }),
                            new Sequence(new List<Node>
                            {
                                new ConditionCanDoAction("atkLight"),
                                new ConditionEnemyDistance(meleeDist),
                                new ActionFaceEnemy(),
                                new ActionLightAtk(),
                            }),
                            new Sequence(new List<Node>
                            {
                                new ConditionEnemyDistance(aggroDist),
                                new Inverter(new ConditionEnemyDistance(0.5f)),
                                new ActionFaceEnemy(),
                                new ActionMoveForward(),
                            })
                        }),
                        new Sequence(new List<Node>
                        {
                            new ActionFaceEnemy(),
                            new ActionMoveForward(),
                            //new ActionWaitTicks(),
                            new ActionStop(),
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
                            new ActionFaceEnemy(),
                            new ActionBlock(),
                        }),
                        // dodge away if we can't block
                        new Sequence(new List<Node>
                        {
                            new ConditionEnemyDistance(meleeDist),
                            new ConditionCanDoAction("dodge"),
                            new ActionBackEnemy(),
                            new ActionDodge(),
                        }),
                        new Sequence(new List<Node>
                        {
                            new Inverter(new ConditionEnemyDistance(4f)),
                            new ActionFaceEnemy(),
                            new ActionMoveForward(),
                            //new ActionWaitTicks(),
                            new ActionStop(),
                        }),
                        // run away until threshold distance
                        new Sequence(new List<Node>
                        {
                            new ConditionEnemyDistance(4f),
                            new ActionBackEnemy(),
                            new ActionMoveForward(),
                            new ActionStop(),
                            new ConditionArenaBounds(1f),
                            new ActionMoveForward(),
                        }),
                        // turn if hitting wall
                        new Sequence(new List<Node>
                        {
                            new ConditionEnemyDistance(3f),
                            new Inverter(new ConditionArenaBounds(1f)),
                            new ActionStop(),
                            new ActionTurnRight(),
                            new ActionMoveForward(),
                            new ConditionArenaBounds(1f),
                        }),
                        new Sequence(new List<Node>
                        {
                            new ConditionEnemyDistance(3f),
                            new Inverter(new ConditionArenaBounds(1f)),
                            new ActionStop(),
                            new ActionTurnLeft(),
                            new ActionMoveForward(),
                            new ConditionArenaBounds(1f),
                        })
                    })
                }),
            }));
        }
        
        public static BTree CreateBob()
        {
            const float rangedDist = 6f;
            const float meleeDist = 2f;
            const float wallDist = 1f;
            const float evadeThresholdHealth = 30f;
            const float evadeThresholdStamina = 20f;
            const float dodgeThresholdStamina = 60f;
            return new BTree(new Selector(new List<Node>
            {
                new ConditionEnemyState("Dead"), // success if enemy is dead
                
                // select defense branch
                new Selector(new List<Node>
                {
                    // defend melee sequence
                    new Sequence(new List<Node>
                    {
                        new ConditionEnemyDistance(meleeDist),
                        new Selector(new List<Node> // if enemy melee
                        {
                            new ConditionEnemyAction("atkLight"),
                            new ConditionEnemyAction("atkHeavy"),
                        }),

                        new Selector(new List<Node>
                        {
                            new Sequence(new List<Node> // try block or
                            {
                                new ConditionCanDoAction("block"),
                                new ActionFaceEnemy(),
                                new ActionBlock(),
                            }),
                            new Sequence(new List<Node> // try dodge
                            {
                                new ConditionCanDoAction("dodge"),
                                new ConditionOwnStamina(dodgeThresholdStamina),
                                new ActionBackEnemy(),
                                new ConditionArenaBounds(wallDist),
                                new ActionMoveForward(),
                                new ActionDodge(),
                            })
                        }),
                    }),

                    // defend ranged sequence
                    new Sequence(new List<Node>
                    {
                        new ConditionEnemyAction("atkRanged"), // if enemy ranged
                        new Selector(new List<Node>
                        {
                            new Sequence(new List<Node> // try block or
                            {
                                new ConditionCanDoAction("block"),
                                new ActionFaceEnemy(),
                                new ActionBlock(),
                            }),
                            new Sequence(new List<Node> // try dodge
                            {
                                new ConditionCanDoAction("dodge"),
                                new ConditionOwnStamina(dodgeThresholdStamina),
                                new ActionBackEnemy(),
                                new ActionTurnLeft(),
                                new ActionMoveForward(),
                                new ActionWaitTicks(),
                                new ActionDodge(),
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
                        new ActionFaceEnemy(),
                        new ActionLightAtk(),
                    }),
                    new Sequence(new List<Node>
                    {
                        new ConditionEnemyDistance(meleeDist),
                        new ConditionCanDoAction("atkHeavy"),
                        new ActionFaceEnemy(),
                        new ActionHeavyAtk(),
                    }),
                }),

                // select movement branch
                new Selector(new List<Node>
                {
                    // evade sequence
                    new Sequence(new List<Node> // evade if less hp than enemy
                    {
                        //new Inverter(new ConditionCompareHealth(-20f)),
                        //new Inverter(new ConditionCompareStamina(-20f)),
                        new ConditionEnemyDistance(rangedDist),
                        new Sequence(new List<Node> // run away until threshold distance
                        {
                            new ActionBackEnemy(),
                            new ActionMoveForward(),
                            new ActionStop(),
                            new ConditionArenaBounds(1f),
                            new ActionMoveForward(),
                        }),

                        new Inverter(new ConditionArenaBounds(1f)),
                        new Selector(new List<Node> // turn if hitting wall
                        {
                            new Sequence(new List<Node>
                            {
                                new ActionStop(),
                                new ActionTurnRight(),
                                new ActionMoveForward(),
                                new ConditionArenaBounds(1f),
                            }),
                            new Sequence(new List<Node>
                            {
                                new ActionStop(),
                                new ActionTurnLeft(),
                                new ActionMoveForward(),
                                new ConditionArenaBounds(1f),
                            }),
                        }),
                    }),
                    // move to melee sequence
                    new Sequence(new List<Node> // stop when melee dist
                    {
                        new ConditionEnemyDistance(meleeDist), // if less
                        //new ConditionOwnHealth(evadeThresholdHealth),
                        //new ConditionCompareHealth(-10f),
                        //new ConditionOwnStamina(evadeThresholdStamina),
                        new ActionFaceEnemy(),
                        new ActionStop(),
                    }),
                    new Sequence(new List<Node> // move until melee dist
                    {
                        new Inverter(new ConditionEnemyDistance(meleeDist)), // if more
                        //new ConditionOwnHealth(evadeThresholdHealth),
                        //new ConditionCompareHealth(-10f),
                        //new ConditionOwnStamina(evadeThresholdStamina),
                        new ActionFaceEnemy(),
                        new ActionMoveForward(),
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
                new ConditionEnemyState("Dead"), // success if enemy is dead
                
                // select defense branch
                new Selector(new List<Node>
                {
                    // defend melee sequence
                    new Sequence(new List<Node>
                    {
                        //new CheckEnemyDistance(meleeDist),
                        new Selector(new List<Node> // if enemy melee
                        {
                            new ConditionEnemyAction("Light Attack"),
                            new ConditionEnemyAction("Heavy Attack"),
                        }),

                        new Selector(new List<Node>
                        {
                            new Sequence(new List<Node> // try block or
                            {
                                new ConditionEnemyDistance(meleeDist),
                                new ConditionCanDoAction("block"),
                                new ActionFaceEnemy(),
                                new ActionBlock(),
                            }),
                            new Sequence(new List<Node> // try dodge
                            {
                                new ConditionEnemyDistance(meleeDist),
                                new ConditionOwnStamina(40f),
                                new ConditionCanDoAction("dodge"),
                                new ActionBackEnemy(),
                                new ActionMoveForward(),
                                new ActionDodge(),
                            }),
                            new Sequence(new List<Node> // try move away
                            {
                                new ConditionEnemyDistance(meleeDist * 2),
                                new ActionBackEnemy(),
                                new Selector(new List<Node>
                                {
                                    new ConditionArenaBounds(1f),
                                    // keep direction if no wall
                                    new Sequence(new List<Node> // if wall try turn left
                                    {
                                        new ActionTurnLeft(),
                                        new ConditionArenaBounds(1f),
                                    }),
                                    new Sequence(new List<Node> // if wall try turn right
                                    {
                                        new ActionTurnRight(),
                                        new ConditionArenaBounds(1f),
                                    }),
                                }),
                                new ActionMoveForward(),
                            }),
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
                                new ActionFaceEnemy(),
                                new ActionBlock(),
                            }),
                            new Sequence(new List<Node> // try dodge
                            {
                                new ConditionOwnStamina(60f),
                                new ConditionCanDoAction("dodge"),
                                new ActionBackEnemy(),
                                new ActionTurnLeft(),
                                new ActionMoveForward(),
                                //new ActionWaitTicks(),
                                new ActionDodge(),
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
                        new ActionFaceEnemy(),
                        new ActionLightAtk(),
                    }),
                    new Sequence(new List<Node> // try dodge into heavyAtk
                    {
                        new ConditionOwnStamina(50f),
                        new ConditionCanDoAction("dodge"),
                        new ConditionCanDoAction("atkHeavy"), // might not have stam for both
                        new Inverter(new ConditionEnemyDistance(3f)),
                        new ConditionEnemyDistance(5f),
                        new ActionFaceEnemy(),
                        new ActionDodge(), // gap close if (3 < dist < 5)
                        //new ActionWaitTicks(),
                        new ConditionEnemyDistance(meleeDist),
                        new ActionHeavyAtk(),
                    }),
                    new Sequence(new List<Node>
                    {
                        new ConditionCanDoAction("atkRanged"),
                        new ConditionEnemyDistance(rangedDist),
                        new ActionFaceEnemy(),
                        new ConditionRangedDirection(30f), // aim good?
                        new ActionRangedAtk(),
                    }),
                }),

                // select movement branch
                new Selector(new List<Node>
                {
                    // evade sequence
                    new Sequence(new List<Node> // evade if less hp than enemy
                    {
                        new Inverter(new ConditionCompareHealth(0f)),
                        new ConditionEnemyDistance(rangedDist),
                        new Sequence(new List<Node> // run away until threshold distance
                        {
                            new ActionBackEnemy(),
                            new ActionMoveForward(),
                            new ActionStop(),
                            new ConditionArenaBounds(1f),
                            new ActionMoveForward(),
                        }),

                        new Inverter(new ConditionArenaBounds(1f)),
                        new Selector(new List<Node> // turn if hitting wall
                        {
                            new Sequence(new List<Node>
                            {
                                new ActionStop(),
                                new ActionTurnRight(),
                                new ActionMoveForward(),
                                new ConditionArenaBounds(1f),
                            }),
                            new Sequence(new List<Node>
                            {
                                new ActionStop(),
                                new ActionTurnLeft(),
                                new ActionMoveForward(),
                                new ConditionArenaBounds(1f),
                            }),
                        }),
                    }),
                    // move to melee sequence
                    new Sequence(new List<Node> // stop when melee dist
                    {
                        new ConditionEnemyDistance(meleeDist), // if less
                        new ConditionOwnHealth(evadeThresholdHealth),
                        new ConditionOwnStamina(evadeThresholdStamina),
                        new ActionFaceEnemy(),
                        new ActionStop(),
                    }),
                    new Sequence(new List<Node> // move until melee dist
                    {
                        new Inverter(new ConditionEnemyDistance(meleeDist)), // if more
                        new ConditionOwnHealth(evadeThresholdHealth),
                        new ConditionOwnStamina(evadeThresholdStamina),
                        new ActionFaceEnemy(),
                        new ActionMoveForward(),
                    }),
                }),
            }));
        }

        public static BTree CreateEmpty()
        {
            return new BTree();
        }

        #endregion
       

    }
}