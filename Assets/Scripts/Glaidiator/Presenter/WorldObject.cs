using System;
using Glaidiator.Model.Collision;
using UnityEngine;

namespace Glaidiator.Presenter
{
    public class WorldObject : MonoBehaviour
    {
        public static WorldObject instance;

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

        private void Update()
        {
            World.Update(Time.deltaTime);
            World.PrintHitboxCount();
        }
    }
}