using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

namespace BasicAI
{
    public class TaskMoveForward : Node
    {

        
        public TaskMoveForward()
        {
            
        }

        public override NodeState Evaluate()
        {

            tree.Move = true;
            
            state = NodeState.RUNNING;
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