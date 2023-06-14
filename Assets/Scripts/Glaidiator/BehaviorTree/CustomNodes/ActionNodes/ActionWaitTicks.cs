using Glaidiator.BehaviorTree.Base;
using Glaidiator.Utils;

namespace Glaidiator.BehaviorTree.CustomNodes.ActionNodes
{
    public class ActionWaitTicks : Action
    {
        private int _totalTicks;
        private int _countTicks;
        
        public ActionWaitTicks(int ticks)
        {
            _totalTicks = ticks;
            _countTicks = _totalTicks;
        }

        public ActionWaitTicks()
        {
            _totalTicks = 2;
            _countTicks = _totalTicks;
        }

        public override NodeState Evaluate() // TODO: Test this
        {
            tree.currentNode = this;
            // Debug.Log("TaskWait tick = " + _countTicks);
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

        #region Genetic Programming
        public override Node Clone()
        {
            return new ActionWaitTicks(_totalTicks);
        }

        public override void Mutate()
        {
            return;
        }

        public override Node Randomized()
        {
            return new ActionWaitTicks(MathStuff.Rand.NextInt(15));
        }
        #endregion
    }
}