using Glaidiator.BehaviorTree.Base;
using Glaidiator.Utils;
using UnityEngine;

namespace Glaidiator.BehaviorTree.CustomNodes.ActionNodes
{
    public class ActionTurnRight : Action
    {
        private int _turnSteps; 
        
        public ActionTurnRight(int turnSteps)
        {
            _turnSteps = turnSteps;
        }

        public ActionTurnRight()
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
            return new ActionTurnRight(_turnSteps);
        }

        public override void Mutate()
        {
            _turnSteps += MathStuff.Rand.NextInt(4) - 2;
            _turnSteps = Mathf.Clamp(_turnSteps, 0, 8);
        }

        public override Node Randomized()
        {
            return new ActionTurnRight(MathStuff.Rand.NextInt(8));
        }
        #endregion
    }
}