using System.Xml;
using Glaidiator.BehaviorTree.Base;

namespace Glaidiator.BehaviorTree.CustomNodes.ConditionNodes
{
    public class ConditionOwnState : Condition
    {
        private string _state;

        public ConditionOwnState(string state)
        {
            _state = state;
        }
        
        /**
         * Checks if current Character-state is equal to given state
         * not to be confused with the returned Node-state.
         */
        public override NodeState Evaluate()
        {
            tree.Active = this;
            string currState = tree.Owner.CurrentState.ToString();

            state = _state == currState ? NodeState.SUCCESS : NodeState.FAILURE;
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

        public override void WriteXml(XmlWriter w)
        {
            throw new System.NotImplementedException();
        }
    }
}