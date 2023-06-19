using Glaidiator.BehaviorTree.Base;
using Glaidiator.Model;
using Glaidiator.Utils;
using UnityEngine;

namespace Glaidiator.BehaviorTree.CustomNodes.ActionNodes
{
    public class ActionSetWp : Action
    {
        private float _distance;
        
        public ActionSetWp(float distance)
        {
            _distance = distance;
        }

        public override NodeState Evaluate()
        {
            tree.Active = this;// for debug info
            Vector3 wp = tree.Owner.Movement.Position + (tree.Direction * _distance);
            SetData("wp", wp);
            ////Debug.Log("set waypoint: " + wp);
            state = NodeState.SUCCESS;
            return state;
        }

        #region Genetic Programming
        public override Node Clone()
        {
            return new ActionSetWp(_distance);
        }

        public override void Mutate()
        {
            _distance += MathStuff.Rand.NextFloat(4f) - 2f;
            _distance = Mathf.Clamp(_distance, 0f, Arena.Diagonal);
        }

        public override Node Randomized()
        {
            return new ActionSetWp(MathStuff.Rand.NextFloat(Arena.Diagonal));
        }
        #endregion
    }
}