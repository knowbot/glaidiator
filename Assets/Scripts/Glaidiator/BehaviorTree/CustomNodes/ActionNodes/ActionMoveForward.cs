using Glaidiator.BehaviorTree.Base;
using UnityEngine;

namespace Glaidiator.BehaviorTree.CustomNodes.ActionNodes
{
    public class ActionMoveForward : Action
    {
        // private readonly float _distance;
        // private readonly bool _hasDistance;
        // private Vector3 _origin;
        
        public ActionMoveForward()
        {
            // _hasDistance = false;
        }

        // public ActionMoveForward(float distance)
        // {
        //     _hasDistance = true;
        //     _distance = distance;
        // }

        public override NodeState Evaluate()
        {
            tree.Active = this; // for debug info

            // if (_hasDistance)
            // {
            //     
            //     var target = GetData("wp");
            //     if (target is not Vector3)
            //     {
            //         tree.Direction = tree.Owner.Movement.Rotation * Vector3.forward;
            //         
            //         target = (tree.Direction * _distance) + tree.Owner.Movement.Position;
            //         SetData("wp", target);
            //         //Debug.Log("MoveForwards new target =" + target);
            //         //Debug.Log("dir: "+tree.Direction);
            //     }
            //     
            //     //tree.Move = true;
            //     //state = NodeState.RUNNING;
            //     
            //     if (Vector3.Distance(tree.Owner.Movement.Position, (Vector3)target) <= 0.01f)
            //     {
            //         ClearData("wp");
            //         state = NodeState.SUCCESS;
            //         //Debug.Log("MoveForwards dist success");
            //     }
            //     else
            //     {
            //         //Debug.Log("moving towards target =" + target);
            //         tree.Move = true;
            //         state = NodeState.RUNNING;
            //     }
            // }
            // else
            {
                ////Debug.Log("move forward DIR = " + tree.Direction);
                tree.Move = true;
                state = NodeState.SUCCESS;
            }
            return state;
        }

        #region Genetic Programming
        public override Node Clone()
        {
            return new ActionMoveForward();
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