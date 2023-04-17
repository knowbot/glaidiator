using System;
using System.Collections;
using System.Collections.Generic;
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

        public void Select() // add statistics param?
        {
            // select a pool of trees based on fitness
            // set new population based on the new generation of offsprings
            // essentially deciding the next generation
        }


        public void Mutate()
        {
            // Mutate all members of population
            foreach(BTree member in _population)
            {
                member.Mutate(mutationChance);
            }
        }

        public void Crossover()
        {
            // for each member of population, select another random member
            // and crossover if the two are not the same tree
            foreach (BTree member in _population)
            {
                BTree mate = _population[Random.Range(0, _population.Count)];
                if (member != mate)
                {
                    member.Crossover(mate, crossoverChance);
                }
            }
        }

        public void CreatePrototypes(BTree tree, Character character) // whatever params needed to init nodes
        {
            prototypesMap = new Dictionary<string, Node>();
            prototypesMap.Add("TaskAttack", new TaskAttack(tree, character));
        }
    }
}