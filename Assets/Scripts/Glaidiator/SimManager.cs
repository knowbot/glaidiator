using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Glaidiator.BehaviorTree;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.BehaviorTree.CustomBTs;
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

        public static float TimeStep = 0.033f;
        public static float MaxDuration = 30f;

        private int _completed = 0;
        private readonly List<Sim> _sims;
        private readonly List<GCHandle> _simHandles;
        private readonly List<JobHandle> _simJobHandles;
        private readonly NativeReference<float>[] _fitnessRef;

        #endregion
        
        #region Singleton
        private SimManager()
        {
            _fitnessRef = new NativeReference<float>[EvoManager.PopulationCapacity];
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

            private bool Finished()
            {
                return owner.IsDead || enemy.IsDead;
            }
            public float Execute()
            {
                float duration = 0f;
                float step = TimeStep;
                owner.SetWorld(world);
                enemy.SetWorld(world);
                while (duration < MaxDuration && !Finished())
                {
                    owner.SetInputs(OInputs.GetInputs());
                    enemy.SetInputs(EInputs.GetInputs());
                    duration += step;
                    owner.Tick(step);
                    enemy.Tick(step);
                    world.Update(step);
                }

                return (((enemy.IsDead && !owner.IsDead) ? 1 : 0) * 1000f // reward wins
                       + owner.Health.Current // reward keeping health
                       + enemy.DamageTaken * 5.0f)  // reward dealing more damage
                       / duration;
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
            _completed = 0;
            for(int i = 0; i < EvoManager.PopulationCapacity; i++)
            {
                _fitnessRef[i] = new NativeReference<float>(0f, Allocator.Persistent);
                _fitnessRef[i].Value = 69f;
                var world = new World();
                var o = new Character(Arena.BossStartPos, Arena.BossStartRot);
                var e = new Character(Arena.PlayerStartPos, Arena.PlayerStartRot);
                var sim = new Sim
                {
                    simID = i,
                    world = world,
                    owner = o,
                    enemy = e,
                    // OInputs = new BTInputProvider(new CustomBobBT(),o, e),
                    OInputs = new BTInputProvider(EvoManager.Instance.Population[i].Clone(),o, e),
                    EInputs = new BTInputProvider(new CustomBobBT(), e, o)
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
            if (!IsDone()) return;

            for (int i = 0; i < _fitnessRef.Length; i++)
            {
                EvoManager.Instance.Population[i].Fitness = _fitnessRef[i].Value;
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
                //EvoManager.Instance.Population[i].Fitness = _fitnessRef[i].Value;
                _fitnessRef[i].Dispose();
            }
        }

        public bool IsDone()
        {
            return _completed == EvoManager.PopulationCapacity;
        }

        public bool IsRunning()
        {
            return _simJobHandles.Count != 0 && !IsDone();
        }
    }
}