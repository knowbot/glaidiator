using Glaidiator.BehaviorTree.Base;

namespace Glaidiator.BehaviorTree.CustomNodes.ActionNodes
{
    public class ActionHeavyAtk : Action
    {
        public override NodeState Evaluate()
        {
            tree.Active = this;// for debug info
            tree.AttackHeavy = true;

            state = NodeState.SUCCESS;
            return state;
        }

        #region Genetic Programming
        public override Node Clone()
        {
            return new ActionHeavyAtk();
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