﻿using System;
using BehaviorTree;

namespace BasicAI
{
    public class TaskDodge : Node
    {
        public TaskDodge(){}

        public override NodeState Evaluate()
        {
            tree.currentNode = this;
            tree.Dodge = true;
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