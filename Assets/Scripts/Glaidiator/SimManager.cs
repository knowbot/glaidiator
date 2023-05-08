using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using BehaviorTree;
using Glaidiator.Model;
using Glaidiator.Model.Collision;
using Glaidiator.Presenter;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using UnityEngine;

namespace Glaidiator
{
    public sealed class SimManager
    {
        #region Fields

        public static float Step => 0.033f;
        public static float MaxDuration => 30f;
        public static readonly int SimCount = 5;

        private int _completed = 0;
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
            private int _outcome;
            public NativeArray<float> fitness;
            public World world;
            public Character player;
            public Character enemy;
            public BTInputProvider PInputs;
            public BTInputProvider EInputs;

            private int Outcome()
            {
                if (player.IsDead) return 1;
                if (enemy.IsDead) return -1;
                return 0;
            }
            public void Execute()
            {
                _outcome = 0;
                _duration = 0f;
                player.SetWorld(world);
                enemy.SetWorld(world);
                while (_duration < MaxDuration && _outcome == 0)
                {
                    player.SetInputs(PInputs.GetInputs());
                    enemy.SetInputs(EInputs.GetInputs());
                    _duration += Step;
                    player.Tick(Step);
                    enemy.Tick(Step);
                    world.Update(Step);
                    _outcome = Outcome();
                }

                fitness[simID] = _outcome * 1000f / _duration + enemy.Health.Current;; // rewards fast wins, slow losses
                // + player.DamageTaken*100f/enemy.DamageTaken - 100f // damage dealt vs damage taken ratio
                 // reward keeping more health at the end
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
            // Debug.Log("Running new batch! Just to check, there are " +
            //           _sims.Count + " sims and " +
            //           _simHandles.Count  +" GCHandles and " +
            //           _simJobHandles.Count + " job handles.");
            _completed = 0;
            _fitnessArray = new NativeArray<float>(SimCount, Allocator.Persistent);
            for(int i = 0; i < SimCount; i++)
            {
                var world = new World();
                var p = new Character(Arena.PlayerStartPos, Arena.PlayerStartRot);
                var e = new Character(Arena.EnemyStartPos, Arena.EnemyStartRot);
                var sim = new Sim
                {
                    simID = i,
                    fitness = _fitnessArray,
                    world = world,
                    player = p,
                    enemy = e,
                    PInputs = new BTInputProvider(p, e),
                    EInputs = new BTInputProvider(e, p)
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
                if (_simJobHandles[i].IsCompleted)
                {
                    Debug.Log("Completed simulation " + _sims[i].simID + " with end fitness " + _fitnessArray[i]);
                    _completed++;
                    _simJobHandles[i].Complete();
                    _simJobHandles.RemoveAt(i);
                    _simHandles[i].Free();
                    _simHandles.RemoveAt(i);
                    _sims.RemoveAt(i);
                }
            }

            if (CheckDone() && _fitnessArray.IsCreated)
            {
                _fitnessArray.Dispose();
            }
        }

        public bool CheckDone()
        {
            return _completed == SimCount;
        }

        public void Destroy()
        {
            if(_fitnessArray.IsCreated)
            {
                _fitnessArray.Dispose();
            }
        }
    }
}