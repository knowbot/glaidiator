using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.BehaviorTree.CustomBTs;
using Glaidiator.BehaviorTree.CustomNodes;
using Glaidiator.BehaviorTree.CustomNodes.CheckNodes;
using Glaidiator.BehaviorTree.CustomNodes.TaskNodes;
using Glaidiator.BehaviorTree.LeafNodes.ConditionNodes;
using Glaidiator.Utils;
using Micosmo.SensorToolkit;
using UnityEngine;
using Random = UnityEngine.Random;
using Serializer = Glaidiator.Utils.Serializer;

namespace Glaidiator
{
    public class EvoManager
    {
        
        private class Candidate
        {
            public BTree tree;
            public float fitness;
        }

        public BTree Champion; 
        public int Era = 0; // # of population resets (to adapt to changing playstyle)
        public int Generation = 0; // # of curr generation
        public const int MaxGenerations = 1000; // # of generations before a reset
        // ? Default values, can be changed in Matrix script
        public static int PopulationCapacity = 50;
        public static int ElitismPct = 10;
        public static float CrossoverFactor = 0.85f;
        public static float MutationFactor = 0.15f;

        public const int MaxChildren = 6;

        public List<BTree> Population;

        private Dictionary<string, Node> _prototypesMap;
        public List<Node> prototypes; // collection of behaviors to sample from
        private List<Composite> _roots;
        
        private readonly string _runPrefix = Guid.NewGuid().ToString();
        private CsvWriter _logger;
        
        #region Singleton
        private EvoManager()
        {
            Champion = new CustomBobBT();
            CreatePrototypes();
            CreateRoots();
            _logger = new CsvWriter($"Test/{_runPrefix}", 
                "fitness", 
                new []{"era","generation","avg","best","worst"},
                ';');
        }
        
        private static readonly Lazy<EvoManager> Lazy = new(() => new EvoManager());
        public static EvoManager Instance => Lazy.Value;
        #endregion

        public void Evaluate()
        {
            var avg = Population.Average(t => t.Fitness);
            var best = Population.Max(t => t.Fitness);
            var worst = Population.Min(t => t.Fitness);
            Debug.Log($"era{Era}, generation {Generation}: avg = {avg} best = {best} worst = {worst}");
            _logger.Write(new[]{Era, Generation, avg, best, worst}.Select(v => v.ToString()).ToArray());
            UpdateChampion();
        }

        public void CrossoverTest()
        {
            var p1 = RandomTree();
            var p2 = RandomTree();
            var c = BTree.Crossover(p1, p2);
            Serializer.Serialize(p1, "parent1");
            Serializer.Serialize(p2, "parent2");
            Serializer.Serialize(c,"child");
        }
        
        public void UpdateChampion()
        {
            BTree newChamp = Population.OrderByDescending(t => t.Fitness).ToArray()[0].Clone();
            if (newChamp.Fitness <= Champion.Fitness) return;
            Champion = newChamp;
            Serializer.Serialize(Champion, "new_champ_era" + Era + "_gen" + Generation, $"Test/{_runPrefix}/Champions/");
        }


        public void Reproduce()
        {
            BTree[] popArray = Population.OrderByDescending(t => t.Fitness).ToArray();
            List<BTree> offspring = new List<BTree>();
            List<BTree> pool = new List<BTree>(); // reproduction pool
            /*
                # ELITISM
                Take top X% of the population based on fitness
                The best 50% elites get passed directly into offspring
                The worst 50% elites have guaranteed slot in reproduction pool
             */
            int elite = (int)(PopulationCapacity * ElitismPct / 100f); // how many elites
            if (elite > 0)
            {
                int top = (elite + 1) / 2; // index to split top/bot 50% elites
                // get elite population
                BTree[] elites = popArray[..elite];
                offspring.AddRange(elites[..top].Select(e => e.Clone()));
                pool.AddRange(elites[top..].Select(e => e.Clone())); // add bot50% elites
            }
            /*
                # FITNESS PROPORTIONATE SELECTION
                Stochastic selection method: the probability for selection of a tree is proportional to its fitness.
                Selected trees are added to a pool which will produce the new generation via genetic operators.
                With added elitism, the worst 50% elites are seeded directly into the pool.
             */
            // calc normalised fitness
            float fitSum = popArray.Sum(tree => tree.Fitness) + 0.0001f; // to avoid division by 0
            List<Candidate> candidates = popArray.Select(tree => new Candidate
                {
                    tree = tree, 
                    fitness = tree.Fitness / fitSum // normalized fitness
                }).ToList();

            // sort candidates list for best fitness first
            candidates.Sort((a, b) => a.fitness.CompareTo(b.fitness));
            // fill pool with roulette wheel selection
            while (pool.Count < PopulationCapacity)
            {
                float accProp = 0f;
                float randVal = Random.value;
                foreach (Candidate c in candidates)
                {
                    if ((c.fitness + accProp) < randVal) pool.Add(c.tree.Clone());
                    accProp += c.fitness;
                }
            }
            /*
               # CROSSOVER AND MUTATION
               Apply crossover and mutation operators to reproduction pool
            */
            while(offspring.Count < PopulationCapacity)
            {
                BTree p1 = pool[Random.Range(0, pool.Count)]; // pick parent 1
                BTree p2 = pool[Random.Range(0, pool.Count)]; // pick parent 2
                BTree child = p1;
                // Crossover chance
                if (MathStuff.Rand.NextFloat() < CrossoverFactor)
                   child = BTree.Crossover(p1, p2);
                if (MathStuff.Rand.NextFloat() < MutationFactor)
                   child.Mutate();
                offspring.Add(child);
            }
            // # INITIALIZE NEW GENERATION
            Population = offspring;
            Generation++;
        }
        
        public void InitPopulation()
        {
            Population = new List<BTree> { Champion };
            while(Population.Count < PopulationCapacity)
                Population.Add(RandomTree());
            Debug.Log(Population.Count);
        }

        public EvoBT RandomTree()
        {
            return new EvoBT(GetRandomRoot().Randomized());
        }
        
        public Node GetRandomRoot()
        {
            return _roots[Random.Range(0, _roots.Count)];
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

        private void CreateRoots()
        {
            _roots = new List<Composite>
            {
                new Selector(),
                new Sequence()
            };
        }
        
         // init prototype samples for random mutations
        private void CreatePrototypes() // whatever params needed to init nodes
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


            prototypes = _prototypesMap.Values.ToList();
        }

        public void NewEra()
        {
            Era++;
            Generation = 0;
            Population.Clear();
            InitPopulation();
        }
    }
}
