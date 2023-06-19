using Glaidiator.BehaviorTree.Base;

namespace Glaidiator.BehaviorTree.CustomNodes.ActionNodes
{
    public class ActionLightAtk : Action
    {
        public override NodeState Evaluate()
        {
            tree.Active = this;// for debug info
            tree.AttackLight = true;

            state = NodeState.SUCCESS;
            return state;
        }

        #region Genetic Programming
        public override Node Clone()
        {
            return new ActionLightAtk();
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