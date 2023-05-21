using System;
using System.Collections.Generic;
using System.Linq;
using Glaidiator.BehaviorTree;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.BehaviorTree.CustomBTs;
using Glaidiator.BehaviorTree.CustomNodes;
using Glaidiator.BehaviorTree.CustomNodes.CheckNodes;
using Glaidiator.BehaviorTree.CustomNodes.TaskNodes;
using Glaidiator.BehaviorTree.LeafNodes.ConditionNodes;
using Glaidiator.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Glaidiator
{
    public class EvoManager
    {
        
        private class Candidate
        {
            public BTree tree;
            public float fitness;
        }
        
        public int generation = 0;
        public int populationCapacity = 2; // OBS: arbitrary test values
        public float crossoverChance = 0.3f;
        public float mutationChance = 0.2f;
        public const int MaxChildren = 6;

        private List<BTree> _population;
        
        public Dictionary<String, Node> prototypesMap;
        public List<Node> prototypes; // collection of behaviors to sample from
        public List<Composite> roots;
        
        #region Singleton
        private EvoManager()
        {
            CreatePrototypes();
            CreateRoots();
        }
        
        private static readonly Lazy<EvoManager> Lazy = new(() => new EvoManager());
        public static EvoManager Instance => Lazy.Value;
        #endregion
        
        
        // selection of new generation
        public void Select() // add statistics param?
        {
            List<BTree> offsprings = new List<BTree>();
            List<Candidate> candidates = new List<Candidate>();
            float fitnessNormal = 0f;
            
            foreach (BTree tree in _population)
            {
                fitnessNormal += tree.GetFitness(); //TODO: Implement fitness
            }

            foreach (BTree tree in _population)
            {
                Candidate c = new Candidate();
                c.tree = tree;
                c.fitness = tree.GetFitness() / fitnessNormal;
                candidates.Add(c);
                // anything domain specific?
            }
            
            // sort candidates list for best fitness first
            candidates.Sort((a, b) => a.fitness.CompareTo(b.fitness));

            // add fit candidates to offsprings using random pruning
            while (offsprings.Count < populationCapacity)
            {
                float accProp = 0f;
                float randVal = Random.value;
                foreach (Candidate c in candidates)
                {
                    if ((c.fitness + accProp) < randVal) offsprings.Add(c.tree.Clone());
                    accProp += c.fitness;
                }
            }

            _population = offsprings;
            generation++;
        }


        // Invoke Mutate on all members of population
        public void Mutate()
        {
            foreach(BTree member in _population)
            {
                member.Mutate(mutationChance);
            }
        }

        // for each member of population, select another random member
        // and crossover if the two are not the same tree
        public void Crossover()
        {
            
            foreach (BTree member in _population)
            {
                //todo: add chance here instead
                BTree mate;
                do
                {
                     mate = _population[Random.Range(0, _population.Count)];
                    if (member != mate)
                       Serializer.Serialize(member.Crossover(mate));

                } while (member == mate);
            }
        }

        public void InitPopulation()
        {
            _population = new List<BTree>();
            for (int i = 0; i < populationCapacity; i++)
            {
                _population.Add(RandomTree());
            }
            Crossover();
            foreach (BTree tree in _population)
            {
                Serializer.Serialize(tree);
            }
        }

        public EvoBT RandomTree()
        {
            return new EvoBT(GetRandomRoot().Randomized());
        }
        
        public Node GetRandomRoot()
        {
            return roots[Random.Range(0, roots.Count)];
        }


        public Node GetRandomNode()
        {
            float p = MathStuff.Rand.NextFloat();
            return p > 0.15 ? GetRandomPrototype() : GetRandomRoot();
        }

        public Node GetRandomPrototype()
        {
            return prototypes[Random.Range(0, prototypes.Count)];
        }

        public void TestRandomGeneration()
        {
            Serializer.Serialize(RandomTree());
        }
        
        private void CreateRoots()
        {
            roots = new List<Composite>
            {
                new Selector(),
                new Sequence()
            };
        }
        
         // init prototype samples for random mutations
        private void CreatePrototypes() // whatever params needed to init nodes
        {
            prototypesMap = new Dictionary<string, Node>();
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
            
            prototypesMap.Add("TaskFaceEnemy", new TaskFaceEnemy());
            prototypesMap.Add("TaskBackEnemy", new TaskBackEnemy());
            prototypesMap.Add("TaskMoveForward", new TaskMoveForward());
            prototypesMap.Add("TaskTurnLeft", new TaskTurnLeft());
            prototypesMap.Add("TaskTurnRight", new TaskTurnRight());
            prototypesMap.Add("CheckArenaBounds", new CheckArenaBounds(1f));
            
            prototypesMap.Add("CheckEnemyDistanceMelee", new CheckEnemyDistance(2f));
            prototypesMap.Add("CheckEnemyDistanceRanged", new CheckEnemyDistance(6f));
            prototypesMap.Add("CheckOwnHealth", new CheckOwnHealth(50f));
            prototypesMap.Add("CheckOwnStamina", new CheckOwnStamina(50f));
            prototypesMap.Add("CheckEnemyHealth", new CheckEnemyHealth(50f));
            prototypesMap.Add("CheckEnemyStamina", new CheckEnemyStamina(50f));
            // prototypesMap.Add("TaskSetWP", new TaskSetWP(2f));
            // prototypesMap.Add("TaskClearWP", new TaskClearWP());
            // prototypesMap.Add("CheckHasWP", new CheckHasTarget("wp"));
            // prototypesMap.Add("CheckWPDistance", new CheckTargetDistance("wp", 0.01f));
            prototypesMap.Add("TaskWait", new TaskWait());

            prototypesMap.Add("CheckEnemyLight", new CheckEnemyAction("atkLight"));
            prototypesMap.Add("CheckEnemyHeavy", new CheckEnemyAction("atkHeavy"));
            prototypesMap.Add("CheckEnemyRanged", new CheckEnemyAction("atkRanged"));
            prototypesMap.Add("CheckEnemyBlock", new CheckEnemyAction("block"));
            prototypesMap.Add("CheckEnemyDodge", new CheckEnemyAction("dodge"));
            prototypesMap.Add("ModuleAtkLight", 
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
            
            prototypesMap.Add("ModuleAtkHeavy", 
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
            
            prototypesMap.Add("ModuleAtkRanged",
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
            
            prototypesMap.Add("ModuleBlock", 
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
            
            prototypesMap.Add("ModuleDodge", 
                new Module(
                    "ModuleDodge", 
                    new Sequence(new List<Node> 
                    {
                        new CheckCanDoAction("dodge"),
                        new TaskDodge(),
                    })
                )
            );


            prototypes = prototypesMap.Values.ToList();
        }
    }
}
