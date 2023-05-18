using System;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Utils;
using UnityEngine;

namespace Glaidiator.BehaviorTree.CustomNodes.TaskNodes
{
    public class TaskTurnRight : Task
    {
        private int _turnSteps; 
        
        public TaskTurnRight(int turnSteps)
        {
            _turnSteps = turnSteps;
        }

        public TaskTurnRight()
        {
            _turnSteps = 2; // 2 equals a 90 degree turn
        }

        public override NodeState Evaluate()
        {
            tree.currentNode = this;// for debug info
            
            Vector3 currDir = tree.Direction;

            if (_turnSteps == 2) // 90 degree turn
            {
                tree.Direction = MathStuff.Get8DDirection(currDir.z, currDir.x * -1);
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
            return new TaskTurnRight(_turnSteps);
        }

        public override void Mutate()
        {
            throw new NotImplementedException();
        }

        public override Node Randomized()
        {
            return new TaskTurnRight(MathStuff.Rand.NextInt(8));
        }
        #endregion
    }
}