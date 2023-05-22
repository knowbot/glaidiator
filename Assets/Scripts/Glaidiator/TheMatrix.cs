using Glaidiator.BehaviorTree;
using UnityEngine;
using UnityEngine.Serialization;

namespace Glaidiator
{
    public class TheMatrix : MonoBehaviour
    { 
        [Header("Evolution Params")]
        [SerializeField] public int populationCapacity = 60;
        [SerializeField] public int elitismPct = 10;
        [SerializeField] public float crossoverFactor = 0.85f;
        [SerializeField]public float mutationFactor = 0.15f;

        [Header("Simulation Params")]
        [SerializeField] public float maxDuration = 30f;
        [SerializeField] public float timeStep = 0.033f;
        
        public static TheMatrix instance;
        private SimManager _simManager;
        private EvoManager _evoManager;
    
        private void Awake(){
            if (instance == null){
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            } else {
                Destroy(this);
            }
            SetEvolutionParameters();
            SetSimulationParameters();
            _simManager.Schedule();
            _evoManager.InitPopulation();
        }

        private void LateUpdate()
        {
            _simManager.Complete();
        }

        private void Update()
        {
            if(_simManager.CheckDone()) _simManager.Schedule();
        }

        private void SetEvolutionParameters()
        {
            _evoManager ??= EvoManager.Instance;
            _evoManager.PopulationCapacity = populationCapacity;
            _evoManager.ElitismPct = elitismPct;
            _evoManager.CrossoverFactor = crossoverFactor;
            _evoManager.MutationFactor = mutationFactor;
        }
        private void SetSimulationParameters()
        {
            _simManager ??= SimManager.Instance;
            _simManager.SimCount = populationCapacity;
            _simManager.MaxDuration = maxDuration;
            _simManager.TimeStep = timeStep;
        }
        

        private void OnDestroy()
        {
            _simManager.ForceComplete();
        }
    }
}