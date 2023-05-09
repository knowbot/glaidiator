using System;
using Glaidiator.Model;
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
            if (instance != null)
                Destroy(gameObject);
            else
                instance = this;
            
            World = new World
            {
                EnableDraw = true
            };
            
    
            DontDestroyOnLoad(gameObject);
        }
        
        private void Update()
        {
            World.Update(Time.deltaTime);
        }
    }
}