using UnityEngine;

namespace Glaidiator
{
    public class TheMatrix : MonoBehaviour
    {
        
        public static TheMatrix instance;
        private SimManager _manager;
    
        private void Awake(){
            if (instance == null){
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            } else {
                Destroy(this);
            }
            
            _manager = SimManager.Instance;
            _manager.Schedule();
        }

        private void LateUpdate()
        {
            _manager.Complete();
        }

        private void Update()
        {
            if(_manager.CheckDone()) _manager.Schedule();
        }

        private void OnDestroy()
        {
            _manager.ForceComplete();
        }
    }
}