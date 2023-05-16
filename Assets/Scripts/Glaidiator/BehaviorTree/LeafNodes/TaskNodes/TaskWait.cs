using System;
using BehaviorTree;

namespace BasicAI
{
    public class TaskWait : Node
    {
        private int _totalTicks;
        private int _countTicks;
        
        public TaskWait(int ticks)
        {
            _totalTicks = ticks;
            _countTicks = _totalTicks;
        }

        public TaskWait()
        {
            _totalTicks = 2;
            _countTicks = _totalTicks;
        }

        public override NodeState Evaluate() // TODO: Test this
        {
            tree.currentNode = this;
            
            _countTicks--;
            if (_countTicks > 0)
            {
                state = NodeState.RUNNING;
            }
            else
            {
                state = NodeState.SUCCESS;
                _countTicks = _totalTicks;
            }
            
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