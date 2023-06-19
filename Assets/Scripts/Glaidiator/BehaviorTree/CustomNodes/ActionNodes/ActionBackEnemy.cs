using Glaidiator.BehaviorTree.Base;
using Glaidiator.Model;
using UnityEngine;

namespace Glaidiator.BehaviorTree.CustomNodes.ActionNodes
{
    public class ActionBackEnemy : Action
    {
        public override NodeState Evaluate()
        {
            tree.Active = this;
            Movement target = ((Character)GetData("enemy"))?.Movement;
            if (target == null)
            {
                state = NodeState.FAILURE;
                return state;
            }

            Vector3 myPos = tree.Owner.Movement.Position;
            tree.Direction = (myPos - target.Position).normalized;

            state = NodeState.SUCCESS;
            return state;
        }

        #region Genetic Programming
        public override Node Clone()
        {
            return new ActionBackEnemy();
        }

        public override void Mutate()
        {
            return;
        }

        public override Node Randomized()
        {
            return Clone();
        }
        #endregion
        
    }
}