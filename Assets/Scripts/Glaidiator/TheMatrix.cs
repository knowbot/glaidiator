﻿using Glaidiator.BehaviorTree;
using UnityEngine;
using UnityEngine.Serialization;

namespace Glaidiator
{
    public class TheMatrix : MonoBehaviour
    { 
        [Header("Evolution Params")]
        [SerializeField] public int populationCapacity = 50;
        [SerializeField][Range(0, 1)] public float variantRatio = 0.5f;
        [SerializeField][Range(0, 1)] public float variantChance = 0.5f;
        [SerializeField][Range(0, 100)] public int elitismPct = 10;
        [SerializeField][Range(0, 1)] public float crossoverFactor = 0.75f;
        [SerializeField][Range(0, 1)] public float mutationFactor = 0.05f;

        [Header("Simulation Params")]
        [SerializeField] public float maxDuration = 30f;
        [SerializeField] public float timeStep = 0.033f;
        
        public static TheMatrix instance;

        private void Awake(){
            if (instance == null){
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            } else {
                Destroy(this);
            }
            SetEvolutionParameters();
            SetSimulationParameters();
            EvoManager.Instance.GenPopulation();
        }

        private void LateUpdate()
        {
            SimManager.Instance.Complete();
            if (!SimManager.Instance.IsDone()) return;
            EvoManager.Instance.Evaluate();
            Plot();
            EvoManager.Instance.Reproduce();
        }

        private void Plot()
        {
            
        }

        private void Update()
        {
            if (EvoManager.Instance.Generation == EvoManager.MaxGenerations) EvoManager.Instance.NewEra();
            if(!SimManager.Instance.IsRunning()) SimManager.Instance.Schedule();
        }

        private void SetEvolutionParameters()
        {
            EvoManager.PopulationCapacity = populationCapacity;
            EvoManager.VariantRatio = variantRatio;
            EvoManager.VariantChance = variantChance;
            EvoManager.ElitismPct = elitismPct;
            EvoManager.CrossoverFactor = crossoverFactor;
            EvoManager.MutationFactor = mutationFactor;
        }
        private void SetSimulationParameters()
        {
            SimManager.MaxDuration = maxDuration;
            SimManager.TimeStep = timeStep;
        }
        

        private void OnDestroy()
        {
            SimManager.Instance.ForceComplete();
        }
    }
}