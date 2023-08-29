using System;
using System.Collections.Generic;
using Glaidiator.BehaviorTree;
using Glaidiator.BehaviorTree.Base;
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
        [SerializeField] public char opponentTree = 'a';
        [SerializeField] public float maxDuration = 30f;
        [SerializeField] public float timeStep = 0.033f;
        [SerializeField] public List<BTree> fixedTrees;

        [Header("Scene References")] [SerializeField]
        public AIContainer fixedAgent;
        public AIContainer adaptiveAgent;

        public static TheMatrix instance;

        private void Awake(){
            if (instance == null){
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            } else {
                Destroy(this);
            }
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
            if (EvoManager.Instance.Generation == EvoManager.MaxGenerations) EvoManager.Instance.NewEra();
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
            SimManager.FixedTree = fixedAgent.Tree.Clone();
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