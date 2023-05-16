using System;
using BehaviorTree;

namespace BasicAI
{
    public class TaskLightAtk : Node
    {
        public TaskLightAtk()
        {
            
        }

        public override NodeState Evaluate()
        {
            tree.currentNode = this;// for debug info
            tree.AttackLight = true;

            state = NodeState.SUCCESS;
            return state;
        }

        public override Node Clone()
        {
            throw new NotImplementedException();
        }

        public override void Mutate()
        {
            throw new NotImplementedException();
        }

        public override Node Randomized()
        {
            throw new NotImplementedException();
        }
    }
}