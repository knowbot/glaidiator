using System;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Utils;
using UnityEngine;

namespace Glaidiator.BehaviorTree.CustomNodes.TaskNodes
{
    public class TaskWait : Task
    {
        private int _totalTicks;
        private int _countTicks;
        
        public TaskWait(int ticks)
        {
            _totalTicks = ticks;
            _countTicks = _totalTicks;
        }

        public TaskWait()
        {
            _totalTicks = 2;
            _countTicks = _totalTicks;
        }

        public override NodeState Evaluate() // TODO: Test this
        {
            tree.currentNode = this;
            // Debug.Log("TaskWait tick = " + _countTicks);
            _countTicks--;
            if (_countTicks > 0)
            {
                state = NodeState.RUNNING;
            }
            else
            {
                state = NodeState.SUCCESS;
                _countTicks = _totalTicks;
            }
            
            return state;
        }

        #region Genetic Programming
        public override Node Clone()
        {
            return new TaskWait(_totalTicks);
        }

        public override void Mutate()
        {
            return;
        }

        public override Node Randomized()
        {
            return new TaskWait(MathStuff.Rand.NextInt(15));
        }
        #endregion
    }
}