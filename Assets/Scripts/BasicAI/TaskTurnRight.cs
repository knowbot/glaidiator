using BehaviorTree;
using UnityEngine;

namespace BasicAI
{
    public class TaskTurnRight : Node
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
            //Vector3 currDir = _ownerCharacter.Movement.LastDir;
            //Vector3 currDir = _ownerCharacter.Movement.Rotation * Vector3.forward;
            
            //Debug.Log("turn right, currDir: "+currDir);
            
            //Vector3 right90 = new Vector3(currDir.z, 0f, currDir.x * -1);
            //Vector3 left90 = new Vector3(currDir.z * -1, 0f,currDir.x); 
            
            //if (_turnSteps == 2) tree.Direction = right90;
            
            //Debug.Log("turn right, new dir = "+tree.Direction);
            
            // wip
            // TODO: implement 45 angle turn

            Vector3 currDir = tree.Direction;
            tree.Direction = new Vector3(currDir.z, 0f, currDir.x * -1);
            Debug.Log("new direction: "+tree.Direction);
            Vector3 wp = _ownerCharacter.Movement.Position + (tree.Direction * 2f);
            Debug.Log("test waypoint: "+wp + "m");
            
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