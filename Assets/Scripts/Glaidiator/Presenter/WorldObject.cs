using System;
using Glaidiator.Model.Collision;
using UnityEngine;

namespace Glaidiator.Presenter
{
    public class WorldObject : MonoBehaviour
    {
        public static WorldObject instance;
        private SimManager _manager;

        public World World { get; private set; }

        private void Awake()
        {
            World = new World();
            if (instance != null)
                Destroy(gameObject);
            else
                instance = this;
    
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _manager = SimManager.Instance;
            //_manager.Init();
        }

        private void Update()
        {
            World.Update(Time.deltaTime);
            //if(_manager.CheckDone()) _manager.Init();
        }

        private void LateUpdate()
        {
            //_manager.Free();
        }

        private void OnDestroy()
        {
            _manager.Destroy();
        }
    }
}