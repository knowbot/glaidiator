using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Glaidiator.Model;

namespace BasicAI
{
    public class TaskMoveForward : Node
    {
        private float _distance;
        private bool _hasDistance;
        private bool _moving = false;
        private Vector3 _origin;
        
        public TaskMoveForward()
        {
            _hasDistance = false;
        }

        public TaskMoveForward(float distance)
        {
            _hasDistance = true;
            _distance = distance;
        }

        public override NodeState Evaluate()
        {
            tree.currentNode = this; // for debug info

            if (_hasDistance)
            {
                
                var target = GetData("wp");
                if (target is not Vector3)
                {
                    tree.Direction = _ownerCharacter.Movement.Rotation * Vector3.forward;
                    
                    target = (tree.Direction * _distance) + _ownerCharacter.Movement.Position;
                    SetData("wp", target);
                    Debug.Log("MoveForwards new target =" + target);
                    Debug.Log("dir: "+tree.Direction);
                }
                
                //tree.Move = true;
                //state = NodeState.RUNNING;
                
                if (Vector3.Distance(_ownerCharacter.Movement.Position, (Vector3)target) <= 0.01f)
                {
                    ClearData("wp");
                    state = NodeState.SUCCESS;
                    Debug.Log("MoveForwards dist success");
                }
                else
                {
                    Debug.Log("moving towards target =" + target);
                    tree.Move = true;
                    state = NodeState.RUNNING;
                }
            }
            else
            {
                //Debug.Log("move forward DIR = " + tree.Direction);
                tree.Move = true;
                state = NodeState.SUCCESS;
            }
            
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