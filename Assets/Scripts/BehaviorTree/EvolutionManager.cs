using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BasicAI;
using BehaviorTree;
using Glaidiator.Model;
using UnityEngine;
using Random = UnityEngine.Random;


namespace BehaviorTree
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
        public void CreatePrototypes(BTree tree, Character character) // whatever params needed to init nodes
        {
            prototypesMap = new Dictionary<string, Node>();
            prototypesMap.Add("TaskLightAtk", new TaskLightAtk());
            
            // TODO: add all node types

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