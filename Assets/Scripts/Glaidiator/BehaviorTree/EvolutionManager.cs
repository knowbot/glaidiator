using System;
using System.Collections.Generic;
using System.Linq;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.BehaviorTree.CustomBTs;
using Glaidiator.BehaviorTree.LeafNodes.ConditionNodes;
using Glaidiator.BehaviorTree.LeafNodes.TaskNodes;
using Random = UnityEngine.Random;

namespace Glaidiator.BehaviorTree
{
    public class EvolutionManager
    {
        
        private class Candidate
        {
            public BTree tree;
            public float fitness;
        }
        
        public int generation = 0;
        public int populationCapacity = 20; // OBS: arbitrary test values
        public float crossoverChance = 0.3f;
        public float mutationChance = 0.2f;

        private List<BTree> _population;
        
        public static Dictionary<String, Node> prototypesMap;
        public static List<Node> prototypes; // collection of behaviors to sample from
        

        public EvolutionManager()
        {
            // init persistant storage (output file)
        }
        
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
                BTree mate = _population[Random.Range(0, _population.Count)];
                if (member != mate)
                {
                    member.Crossover(mate, crossoverChance);
                }
            }
        }

        // init prototype samples for random mutations
        public void CreatePrototypes() // whatever params needed to init nodes
        {
            prototypesMap = new Dictionary<string, Node>();
            prototypesMap.Add("TaskLightAtk", new TaskLightAtk());
            prototypesMap.Add("TaskHeavyAtk", new TaskHeavyAtk());
            prototypesMap.Add("TaskRangedAtk", new TaskRangedAtk());
            prototypesMap.Add("TaskBlock", new TaskBlock());
            prototypesMap.Add("TaskDodge", new TaskDodge());
            prototypesMap.Add("CheckLightAtk", new ConditionCanDoAction("atkLight"));
            prototypesMap.Add("CheckHeavyAtk", new ConditionCanDoAction("atkHeavy"));
            prototypesMap.Add("CheckRangedAtk", new ConditionCanDoAction("atkRanged"));
            prototypesMap.Add("CheckBlock", new ConditionCanDoAction("block"));
            prototypesMap.Add("CheckDodge", new ConditionCanDoAction("dodge"));
            
            prototypesMap.Add("TaskFaceEnemy", new TaskFaceEnemy());
            prototypesMap.Add("TaskBackEnemy", new TaskBackEnemy());
            prototypesMap.Add("TaskMoveForward", new TaskMoveForward());
            prototypesMap.Add("TaskTurnLeft", new TaskTurnLeft());
            prototypesMap.Add("TaskTurnRight", new TaskTurnRight());
            prototypesMap.Add("CheckArenaBounds", new ConditionArenaBounds(1f));
            
            prototypesMap.Add("CheckEnemyDistanceMelee", new ConditionEnemyDistance(2f));
            prototypesMap.Add("CheckEnemyDistanceRanged", new ConditionEnemyDistance(6f));
            prototypesMap.Add("CheckEnemyDistanceAggro", new ConditionEnemyDistance(8f));
            
            prototypesMap.Add("CheckOwnHealth", new ConditionOwnHealth(50f));
            prototypesMap.Add("CheckOwnStamina", new ConditionOwnStamina(50f));
            prototypesMap.Add("CheckEnemyHealth", new ConditionEnemyHealth(50f));
            prototypesMap.Add("CheckEnemyStamina", new ConditionEnemyStamina(50f));
            
            prototypesMap.Add("TaskSetWP", new TaskSetWP(2f));
            prototypesMap.Add("TaskClearWP", new TaskClearWP());
            prototypesMap.Add("CheckHasWP", new ConditionHasTarget("wp"));
            prototypesMap.Add("CheckWPDistance", new ConditionTargetDistance("wp", 0.01f));
            
            prototypesMap.Add("TaskWait", new TaskWait());

            prototypesMap.Add("CheckEnemyLight", new ConditionEnemyAction("Light Attack"));
            prototypesMap.Add("CheckEnemyHeavy", new ConditionEnemyAction("Heavy Attack"));
            prototypesMap.Add("CheckEnemyRanged", new ConditionEnemyAction("Ranged Attack"));
            prototypesMap.Add("CheckEnemyBlock", new ConditionEnemyAction("Block"));
            prototypesMap.Add("CheckEnemyDodge", new ConditionEnemyAction("Dodge"));
            
            Node seqLightAtk = new Sequence(new List<Node>
            {
                new ConditionCanDoAction("atkLight"),
                new TaskFaceEnemy(),
                new TaskLightAtk(),
            });
            prototypesMap.Add("SeqLightAttack", seqLightAtk);
            
            Node seqHeavyAtk = new Sequence(new List<Node>
            {
                new ConditionCanDoAction("atkHeavy"),
                new TaskFaceEnemy(),
                new TaskLightAtk(),
            });
            prototypesMap.Add("SeqHeavyAttack", seqHeavyAtk);

            Node seqRangedAtk = new Sequence(new List<Node>
            {
                new Inverter(new ConditionEnemyDistance(3f)),
                new ConditionCanDoAction("atkRanged"),
                new ConditionEnemyDistance(6f),
                new TaskFaceEnemy(),
                new ConditionRangedDirection(30f),
                new TaskRangedAtk(),
            });
            prototypesMap.Add("SeqRangedAttack", seqRangedAtk);
            
            Node seqBlock = new Sequence(new List<Node>
            {
                new ConditionEnemyDistance(2f),
                new ConditionCanDoAction("block"),
                new TaskFaceEnemy(),
                new TaskBlock(),
            });
            prototypesMap.Add("SeqBlock", seqBlock);

            
            prototypes = prototypesMap.Values.ToList();
        }

        public void InitPopulation()
        {
            _population = new List<BTree>();
            for (int i = 0; i < populationCapacity; i++)
            {
                Node root = GetRandomRootNode().Randomized();
                BTree tree = new EvoBT(root);
                _population.Add(tree);
            }
        }

        public static Node GetNewRandomNode()
        {
            return prototypes[Random.Range(0, prototypes.Count)];
        }

        public Node GetRandomRootNode()
        {
            // define list with valid root nodes
            List<Node> nodes = new List<Node>();
            nodes.Add(new Selector());
            nodes.Add(new Sequence());

            return nodes[Random.Range(0, nodes.Count)];
        }
    }
}
