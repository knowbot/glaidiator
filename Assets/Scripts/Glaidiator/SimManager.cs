using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
        #region Fields

        public static float Step => 0.0167f;
        public static float MaxDuration => 30f;
        public static readonly int SimCount = 20;
        
        private readonly List<Sim> _sims;
        private readonly List<GCHandle> _simHandles;
        private readonly List<JobHandle> _simJobHandles;
        private NativeArray<float> _fitnessArray;


        #endregion
        
        #region Singleton
        private SimManager()
        {
            _sims = new List<Sim>();
            _simHandles = new List<GCHandle>();
            _simJobHandles = new List<JobHandle>();
        }
        
        private static readonly Lazy<SimManager> Lazy = new(() => new SimManager());
        public static SimManager Instance => Lazy.Value;
        #endregion

        #region Structs

        private interface IManagedJob
        {
            void Execute();
        }

        private struct Sim : IManagedJob
        {    
            public int simID;
            private float _duration;
            public NativeArray<float> fitness;
            public World world;
            public Character player;
            public Character enemy;
            public void Execute()
            {
                _duration = 0f;
                while (_duration < MaxDuration)
                {
                    world.Update(Step);
                    fitness[simID] += 1.0f;
                    _duration += Step;
                }
            }
        }

        private struct SimJob : IJob
        {
            public GCHandle handle;
            public void Execute() {
                var sim = (IManagedJob) handle.Target;
                sim.Execute();
            }
        }
        #endregion
        
       

        public void Init()
        {
            _fitnessArray = new NativeArray<float>(SimCount, Allocator.Persistent);
            for(int i = 0; i < SimCount; i++)
            {
                var world = new World();
                var sim = new Sim
                {
                    simID = i,
                    fitness = _fitnessArray,
                    world = world,
                    player = new Character(world, Arena.PlayerStartPos, Arena.PlayerStartRot),
                    enemy = new Character(world, Arena.EnemyStartPos, Arena.EnemyStartRot)
                };
                GCHandle simHandle = GCHandle.Alloc(sim);
                var simJob = new SimJob
                {
                    handle = simHandle
                };
                JobHandle simJobHandle = simJob.Schedule();
                _sims.Add(sim);
                _simHandles.Add(simHandle);
                _simJobHandles.Add(simJobHandle);
            }
        }

        public void Free()
        {
            for (int i = _simJobHandles.Count - 1; i >= 0; --i)
            {
                if (!_simJobHandles[i].IsCompleted) continue;
                Debug.Log("Completed simulation " + _sims[i].simID + " with end fitness " + _fitnessArray[i]);
                _simJobHandles[i].Complete();
                _simJobHandles.RemoveAt(i);
                _simHandles[i].Free();
                _simHandles.RemoveAt(i);
            }
            if (_simJobHandles.Count == 0 && _fitnessArray.IsCreated) _fitnessArray.Dispose();
        }

        public bool CheckDone()
        {
            return _simJobHandles.Count == 0;
        }

    }
}