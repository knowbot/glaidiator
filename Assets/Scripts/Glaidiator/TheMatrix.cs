using Glaidiator.BehaviorTree;
using UnityEngine;

namespace Glaidiator
{
    public class TheMatrix : MonoBehaviour
    {
        
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
            
            _simManager = SimManager.Instance;
            _evoManager = EvoManager.Instance;
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

        private void OnDestroy()
        {
            _simManager.ForceComplete();
        }
    }
}