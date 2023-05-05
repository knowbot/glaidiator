using BehaviorTree;

namespace BasicAI
{
    public class CheckHasWP : Node
    {
        public CheckHasWP()
        {
            
        }

        public override NodeState Evaluate()
        {
            tree.currentNode = this;
            
            state = GetData("wp") != null ? NodeState.SUCCESS : NodeState.FAILURE;

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