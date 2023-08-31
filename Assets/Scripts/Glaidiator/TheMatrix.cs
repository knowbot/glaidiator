using System;
using System.Collections.Generic;
using Glaidiator.BehaviorTree;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Glaidiator
{
    public class TheMatrix : MonoBehaviour
    { 
        [Header("Evolution Params")]
        [SerializeField] public int populationCapacity = 50;
        [SerializeField][Range(0, 1)] public float variantRatio = 0.5f;
        [SerializeField][Range(0, 1)] public float variantMutateChance = 0.5f;
        [SerializeField][Range(0, 100)] public int elitismPct = 10;
        [SerializeField][Range(0, 1)] public float crossoverFactor = 0.75f;
        [SerializeField][Range(0, 1)] public float mutationFactor = 0.05f;

        [Header("Simulation Params")]
        [SerializeField] public float maxDuration = 30f;
        [SerializeField] public float timeStep = 0.033f;
        private readonly List<BTree> fixedTrees = new List<BTree>();

        [Header("Scene References")] [SerializeField]
        public AIContainer fixedAgent;
        public AIContainer adaptiveAgent;
        public bool cycleOpponentTrees = false;
        public bool randomizeCycle = true;

        public static TheMatrix instance;

        private void Awake(){
            if (instance == null){
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            } else {
                Destroy(this);
            }
            fixedTrees.Add(BTreeFactory.CreateBob());
            fixedTrees.Add(BTreeFactory.CreateCharlie());
            fixedTrees.Add(BTreeFactory.CreateAshley());
            
            foreach (var tree in fixedTrees)
            {
                Debug.Log("Adding opponent " + tree.Name + " to the opponent list with id " + tree.Name.GetHashCode());
            }
            if(randomizeCycle) fixedTrees.Shuffle();
        }

        private void Start()
        {
            SetEvolutionParameters();
            SetSimulationParameters();
            EvoManager.Instance.GenPopulation();
        }

        private void LateUpdate()
        {
            SimManager.Instance.Complete();
            if (!SimManager.Instance.IsDone()) return;
            EvoManager.Instance.Evaluate();
            EvoManager.Instance.Reproduce();
        }
        

        private void Update()
        {
            if (EvoManager.Instance.HasNewChampion)
            {
                Debug.Log("Adaptive agent BT was updated!");
                adaptiveAgent.Tree = EvoManager.Instance.Champion.Clone();
                EvoManager.Instance.HasNewChampion = false;
            }

            if (EvoManager.Instance.Generation == EvoManager.MaxGenerations)
            {
                EvoManager.Instance.NewEra();
                if(EvoManager.Instance.Era % fixedTrees.Count == 0 && randomizeCycle)
                    fixedTrees.Shuffle();
                if (cycleOpponentTrees)
                {
                    BTree newOpp = fixedTrees[EvoManager.Instance.Era % fixedTrees.Count];
                    SimManager.FixedTree = newOpp.Clone();
                    fixedAgent.Tree = newOpp.Clone();
                    print("Now evaluating against " + newOpp.Name);
                }
            }
            if(!SimManager.Instance.IsRunning()) SimManager.Instance.Schedule();
        }

        private void SetEvolutionParameters()
        {
            EvoManager.Instance.Champion = adaptiveAgent.Tree.Clone();
            EvoManager.PopulationCapacity = populationCapacity;
            EvoManager.VariantRatio = variantRatio;
            EvoManager.VariantMutateChance = variantMutateChance;
            EvoManager.ElitismPct = elitismPct;
            EvoManager.CrossoverFactor = crossoverFactor;
            EvoManager.MutationFactor = mutationFactor;
        }
        private void SetSimulationParameters()
        {
            SimManager.FixedTree = cycleOpponentTrees ? fixedTrees[0].Clone() : fixedAgent.Tree.Clone();
            print("Now evaluating against " + SimManager.FixedTree.Name);
            SimManager.MaxDuration = maxDuration;
            SimManager.TimeStep = timeStep;
        }

        private void ResetLiveGame()
        {
            
        }

        private void OnDestroy()
        {
            SimManager.Instance.ForceComplete();
        }

        private void OnDisable()
        {
            SimManager.Instance.ForceComplete();
        }

        public void ResetActiveTrees()
        {
            adaptiveAgent.Tree.Init();
            fixedAgent.Tree.Init();
        }
    }
}