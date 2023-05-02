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
            //Vector3 currDir = _ownerCharacter.Movement.LastDir;
            //Vector3 currDir = _ownerCharacter.Movement.Rotation * Vector3.forward;
            Vector3 currDir = tree.Direction;
            Debug.Log("turn right, currDir: "+currDir);
            
            Vector3 right90 = new Vector3(currDir.z, 0f, currDir.x * -1);
            Vector3 left90 = new Vector3(currDir.z * -1, 0f,currDir.x); 
            
            if (_turnSteps == 2) tree.Direction = right90;
            Debug.Log("turn right, new dir = "+tree.Direction);
            
            // wip
            
            // TODO: implement 45 angle turn

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