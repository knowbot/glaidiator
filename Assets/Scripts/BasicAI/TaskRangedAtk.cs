﻿using BehaviorTree;

namespace BasicAI
{
    public class TaskRangedAtk : Node
    {
        public TaskRangedAtk(){}

        public override NodeState Evaluate()
        {
            tree.currentNode = this;
            tree.AttackRanged = true;
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