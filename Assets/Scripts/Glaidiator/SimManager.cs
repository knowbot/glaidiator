using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Glaidiator.BehaviorTree;
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

        public float TimeStep = 0.033f;
        public float MaxDuration = 30f;
        public int SimCount = 5;

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
            public World world;
            public Character owner;
            public Character enemy;
            public BTInputProvider OInputs;
            public BTInputProvider EInputs;

            private int Outcome()
            {
                if (enemy.IsDead) return 1;
                if (owner.IsDead) return -1;
                return 0;
            }
            public float Execute()
            {
                int outcome = 0;
                float duration = 0f;
                float step = Instance.TimeStep;
                owner.SetWorld(world);
                enemy.SetWorld(world);
                while (duration < Instance.MaxDuration && outcome == 0)
                {
                    owner.SetInputs(OInputs.GetInputs());
                    enemy.SetInputs(EInputs.GetInputs());
                    duration += step;
                    owner.Tick(step);
                    enemy.Tick(step);
                    world.Update(step);
                    outcome = Outcome();
                }

                return outcome * 5000f / duration // reward fast wins/slow losses
                    + owner.Health.Current / 10f // reward keeping more health
                    + owner.DamageTaken * 100f / enemy.DamageTaken - 100f; // damage dealt vs damage taken ratio
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
                var e = new Character(Arena.BossStartPos, Arena.BossStartRot);
                var sim = new Sim
                {
                    simID = i,
                    world = world,
                    owner = p,
                    enemy = e,
                    OInputs = new BTInputProvider(e, p),
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