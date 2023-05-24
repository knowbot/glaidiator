using System;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Utils;
using UnityEngine;

namespace Glaidiator.BehaviorTree.CustomNodes.TaskNodes
{
    public class TaskTurnLeft : Task
    {
        private int _turnSteps;
        
        public TaskTurnLeft(int turnSteps)
        {
            _turnSteps = turnSteps;
        }

        public TaskTurnLeft()
        {
            _turnSteps = 2;
        }
        
        public override NodeState Evaluate()
        {
            tree.currentNode = this;// for debug info
            
            Vector3 currDir = tree.Direction;

            if (_turnSteps == 2) // 90 degree turn
            {
                tree.Direction = MathStuff.Get8DDirection(currDir.z * -1, currDir.x);
            }
            else // 45 degree turn * turn steps
            {
                
            }
            
            state = NodeState.SUCCESS;
            return state;
        }

        #region Genetic Programming
        public override Node Clone()
        {
            return new TaskTurnLeft(_turnSteps);
        }

        public override void Mutate()
        {
            _turnSteps += MathStuff.Rand.NextInt(4) - 2;
            _turnSteps = Mathf.Clamp(_turnSteps, 0, 8);
        }

        public override Node Randomized()
        {
            return new TaskTurnLeft(MathStuff.Rand.NextInt(8));
        }
        #endregion
    }
}