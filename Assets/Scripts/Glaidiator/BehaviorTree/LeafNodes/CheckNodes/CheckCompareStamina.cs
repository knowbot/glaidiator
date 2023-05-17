using Glaidiator.BehaviorTree.Base;
using Glaidiator.Model;

namespace Glaidiator.BehaviorTree.LeafNodes.CheckNodes
{
    public class CheckCompareStamina : Node
    {
        public override NodeState Evaluate()
        {
            tree.currentNode = this;
            
            Character enemy = (Character)GetData("enemy");
            if (enemy == null)
            {
                state = NodeState.FAILURE;
                return state;
            }

            state = enemy.Stamina.Current < owner.Stamina.Current ? NodeState.SUCCESS : NodeState.FAILURE;
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