using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Glaidiator.BehaviorTree;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Utils;
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
        public bool HasNewChampion = false;
        public int Era = 0; // # of population resets (to adapt to changing playstyle)
        public int Generation = 0; // # of curr generation
        public const int MaxGenerations = 500; // # of generations before a reset
        // ? Default values, can be changed in Matrix script
        public static float VariantRatio = 0.5f;
        public static float VariantMutateChance = 0.5f;
        public static int PopulationCapacity = 50;
        public static int ElitismPct = 10;
        public static float CrossoverFactor = 0.85f;
        public static float MutationFactor = 0.15f;

        public const int MaxChildren = 6;

        public List<BTree> Population;
        
        private readonly string _runPrefix = Guid.NewGuid().ToString();
        private readonly CsvWriter _logger;
        
        #region Singleton
        private EvoManager()
        {
            _logger = new CsvWriter($"Test~/{_runPrefix}", 
                "fitness", 
                new []{"era","generation","opponent","avg","best","worst","avg_depth","max_depth","min_depth","avg_size","max_size","min_size","champ_depth","champ_size"},
                ';');
        }
        
        private static readonly Lazy<EvoManager> Lazy = new(() => new EvoManager());
        public static EvoManager Instance => Lazy.Value;
        #endregion

        public void Evaluate()
        {
            var avgFitness = Population.Average(t => t.Fitness);
            var maxFitness = Population.Max(t => t.Fitness);
            var minFitness = Population.Min(t => t.Fitness);            
            var avgDepth = Population.Average(t => t.Root.GetDepth());
            var maxDepth = Population.Max(t => t.Root.GetDepth());
            var minDepth = Population.Min(t => t.Root.GetDepth());
            var avgSize = Population.Average(t => t.Root.GetSize());
            var maxSize = Population.Max(t => t.Root.GetSize());
            var minSize = Population.Min(t => t.Root.GetSize());
            Debug.Log($"era{Era}, generation {Generation}: avg = {avgFitness} best = {maxFitness} worst = {minFitness}");
            // Debug.Log("zeroIndex " + Population.FindIndex(t=> t.Fitness == 0));
            _logger.Write(new[]
            {
                Era, 
                Generation,
                SimManager.FixedTree.Name.GetHashCode(),
                avgFitness, 
                maxFitness,
                minFitness, 
                avgDepth, 
                maxDepth, 
                minDepth,
                avgSize,
                maxSize,
                minSize,
                Champion.Root.GetDepth(),
                Champion.Root.GetSize()
            }.Select(v => v.ToString("0.0000")).ToArray());
            UpdateChampion();
        }

        public void UpdateChampion()
        {
            BTree newChamp = Population.OrderByDescending(t => t.Fitness).ToArray()[0];
            if (newChamp.Fitness <= Champion.Fitness) return;
            HasNewChampion = true;
            Debug.Log($"New champ at gen {Generation} with fitness {newChamp.Fitness}");
            Champion = newChamp.Clone();
            Serializer.Serialize(Champion, $"era{Era}_gen{Generation}_tree_{Population.IndexOf(newChamp)}", $"Test~/{_runPrefix}/Champions/");
        }


        public void Reproduce()
        {
            if(Generation == 0) SaveGeneration();
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
            // windowing to make fitness positive
            float minFit = popArray.Min(tree => tree.Fitness);
            minFit = (minFit < 0) ? Math.Abs(minFit + 0.01f) : 0f;
            float fitSum = popArray.Sum(tree => tree.Fitness + minFit); // to avoid division by 0
            List<Candidate> candidates = popArray.Select(tree => new Candidate
                {
                    tree = tree, 
                    fitness = (tree.Fitness + minFit) / fitSum // normalized fitness
                }).ToList();

            // sort candidates list for best fitness first
            candidates.Sort((a, b) => a.fitness.CompareTo(b.fitness));
            // fill pool with roulette wheel selection
            while (pool.Count < PopulationCapacity)
            {
                var accProp = 0f;
                var randVal = Random.value;
                foreach (Candidate c in candidates)
                {
                    if ((c.fitness + accProp) > randVal) pool.Add(c.tree.Clone());
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

        // Mixed r
        public void GenPopulation()
        {
            Population = new List<BTree> { Champion };
            int i = 0;
            while (Population.Count < PopulationCapacity)
            {
                i++;
                BTree tree = (float)i / PopulationCapacity < VariantRatio 
                    ? BTreeFactory.CreateVariant(Champion, VariantMutateChance) 
                    : BTreeFactory.CreateRandom();
                Population.Add(tree);
            }
        }

        public void NewEra()
        {
            Era++;
            Generation = 0;
            Population.Clear();
            GenPopulation();
        }

        private void SaveGeneration()
        {
            for (int i = 0; i < PopulationCapacity; i++)
                Serializer.Serialize(Population[i], $"tree_{i}", $"Test~/{_runPrefix}/Generation{Generation}");
        }
    }
}
