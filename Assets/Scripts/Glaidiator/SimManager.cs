using System;
using System.Collections.Generic;
using Glaidiator.Model;
using Glaidiator.Model.Collision;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using UnityEngine;

namespace Glaidiator
{
    public sealed class SimManager
    {
        #region Singleton
        private SimManager()
        {
        }
        private static readonly Lazy<SimManager> Lazy = new(() => new SimManager());
        public static SimManager Instance => Lazy.Value;
        public static float Step => 0.0167f;
        public static float MaxDuration => 30f;
        public static int SimCount => 20;
        private List<SimJob> _simJobs;
        private List<JobHandle> _jobHandles;
        private NativeArray<float> _fitnessArray;
        #endregion
        
        [BurstCompile]
        public struct SimJob : IJob
        {
            public NativeReference<float> duration;
            public NativeReference<float> fitness;
            public Character player;
            public Character enemy;
            public World world;
            
            public void Execute()
            {
                while (duration.Value < MaxDuration)
                {
                    duration.Value += 1.0f;
                    fitness.Value += 2.0f;
                }
                // player.Tick(Step);
                // enemy.Tick(Step);
                // world.Update(Step);
            }
        }

        public void Init()
        {
            while (_simJobs.Count < SimCount)
            {
                var world = new World();
                var newSim = new SimJob
                {
                    world = world,
                    player = new Character(world, Arena.PlayerStartPos, Arena.PlayerStartRot),
                    enemy = new Character(world, Arena.EnemyStartPos, Arena.EnemyStartRot),
                };
                _simJobs.Add(newSim);
            }
        }
        
        

    }
}