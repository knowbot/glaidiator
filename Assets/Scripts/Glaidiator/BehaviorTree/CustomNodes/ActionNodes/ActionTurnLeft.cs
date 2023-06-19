using System;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Utils;
using UnityEngine;

namespace Glaidiator.BehaviorTree.CustomNodes.ActionNodes
{
    public class ActionTurnLeft : Action
    {
        private int _turnSteps;
        
        public ActionTurnLeft(int turnSteps)
        {
            _turnSteps = turnSteps;
        }

        public ActionTurnLeft()
        {
            _turnSteps = 2;
        }
        
        public override NodeState Evaluate()
        {
            tree.Active = this;// for debug info
            
            Vector3 currDir = tree.Owner.Movement.LastDir;

            int currInd = Direction.GetIndex(currDir);
            if (currInd == -1)
                throw new Exception("Current direction is not discretized!");

            for (int i = 0; i < _turnSteps; i++)
            {
                if (currInd == 0) currInd = 7;
                else currInd--;
            }

            tree.Direction = Direction.Get(currInd);
            
            state = NodeState.SUCCESS;
            return state;
        }

        #region Genetic Programming
        public override Node Clone()
        {
            return new ActionTurnLeft(_turnSteps);
        }

        public override void Mutate()
        {
            _turnSteps += MathStuff.Rand.NextInt(4) - 2;
            _turnSteps = Mathf.Clamp(_turnSteps, 0, 8);
        }

        public override Node Randomized()
        {
            return new ActionTurnLeft(MathStuff.Rand.NextInt(8));
        }
        #endregion
    }
}