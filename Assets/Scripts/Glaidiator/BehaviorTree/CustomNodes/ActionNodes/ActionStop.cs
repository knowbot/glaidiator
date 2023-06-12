using Glaidiator.BehaviorTree.Base;

namespace Glaidiator.BehaviorTree.CustomNodes.ActionNodes
{
    public class ActionStop : Action
    {
        public override NodeState Evaluate()
        {
            tree.currentNode = this;
            
            tree.Move = false;
            tree.Block = false;
            tree.Dodge = false;
            tree.AttackHeavy = false;
            tree.AttackLight = false;
            tree.AttackRanged = false;
            state = NodeState.SUCCESS;
            return state;
        }

        #region Genetic Programming
        public override Node Clone()
        {
            return new ActionStop();
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