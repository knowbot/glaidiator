using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using BehaviorTree;
using Glaidiator.Model;
using Glaidiator.Model.Collision;
using Unity.Collections;
using Unity.Jobs;
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
        private readonly NativeReference<float>[] _fitnessRef;
        public float[] Fitness { get; private set; }

        #endregion
        
        #region Singleton
        private SimManager()
        {
            _fitnessRef = new NativeReference<float>[SimCount];
            Fitness = new float[SimCount];
            _sims = new List<Sim>();
            _simHandles = new List<GCHandle>();
            _simJobHandles = new List<JobHandle>();
        }
        
        private static readonly Lazy<SimManager> Lazy = new(() => new SimManager());
        public static SimManager Instance => Lazy.Value;
        #endregion

        #region Structs

        private interface IManagedJob<out T> where T : unmanaged
        {
            T Execute();
        }

        private struct Sim : IManagedJob<float>
        {    
            public int simID;
            private float _duration;
            private int _outcome;
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
            public float Execute()
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

                return _outcome * 10000f / _duration + player.Health.Current; // rewards fast wins, slow losses
                // + player.DamageTaken*100f/enemy.DamageTaken - 100f // damage dealt vs damage taken ratio
                 // reward keeping more health at the end
            }
        }

        private struct SimJob : IJob
        {
            public GCHandle handle;
            public NativeReference<float> fitness;
            public void Execute() {
                var sim = (Sim)handle.Target;
                fitness.Value = sim.Execute();
            }
        }
        #endregion
        
        public void Schedule()
        {
            // Debug.Log("Running new batch! Just to check, there are " +
            //           _sims.Count + " sims and " +
            //           _simHandles.Count  +" GCHandles and " +
            //           _simJobHandles.Count + " job handles.");
            _completed = 0;
            for(int i = 0; i < SimCount; i++)
            {
                Fitness[i] = 0f;
                _fitnessRef[i] = new NativeReference<float>(0f, Allocator.Persistent);
                var world = new World();
                var p = new Character(Arena.PlayerStartPos, Arena.PlayerStartRot);
                var e = new Character(Arena.EnemyStartPos, Arena.EnemyStartRot);
                var sim = new Sim
                {
                    simID = i,
                    world = world,
                    player = p,
                    enemy = e,
                    PInputs = new BTInputProvider(p, e),
                    EInputs = new BTInputProvider(e, p)
                };
                
                GCHandle simHandle = GCHandle.Alloc(sim);
                var simJob = new SimJob
                { 
                    handle = simHandle,
                    fitness = _fitnessRef[i]
                };
                
                JobHandle simJobHandle = simJob.Schedule();
                _sims.Add(sim);
                _simHandles.Add(simHandle);
                _simJobHandles.Add(simJobHandle);
            }
        }

        public void Complete()
        {
            for (int i = _simJobHandles.Count - 1; i >= 0; --i)
            {
                if (!_simJobHandles[i].IsCompleted) continue;
                _completed++;
                _simJobHandles[i].Complete();
                _simJobHandles.RemoveAt(i);
                _simHandles[i].Free();
                _simHandles.RemoveAt(i);
                _sims.RemoveAt(i);
            }
            if (!CheckDone()) return;
            
            for (int i = 0; i < SimCount; i++)
                if (_fitnessRef[i].IsCreated)
                {
                    Fitness[i] = _fitnessRef[i].Value;
                    Debug.Log("sim ended with fitness " + Fitness[i]);
                    _fitnessRef[i].Dispose();
                }
        }

        public void ForceComplete()
        {
            for (int i = _simJobHandles.Count - 1; i >= 0; --i)
            {
                _completed++;
                _simJobHandles[i].Complete();
                _simJobHandles.RemoveAt(i);
                _simHandles[i].Free();
                _simHandles.RemoveAt(i);
                _sims.RemoveAt(i);
                if (!_fitnessRef[i].IsCreated) continue;
                Fitness[i] = _fitnessRef[i].Value;
                _fitnessRef[i].Dispose();
            }
        }

        public bool CheckDone()
        {
            return _completed == SimCount;
        }
    }
}