using BehaviorTree;
using Glaidiator.Model.Utils;
using UnityEngine;

namespace BasicAI
{
    public class TaskTurnLeft : Node
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

        public override Node Clone()
        {
            throw new System.NotImplementedException();
        }

        public override void Mutate()
        {
            throw new System.NotImplementedException();
        }

        public override Node Randomized()
        {
            throw new System.NotImplementedException();
        }
    }
}