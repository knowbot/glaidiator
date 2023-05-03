using BehaviorTree;

namespace BasicAI
{
    public class TaskClearWP : Node
    {

        public TaskClearWP()
        {
            
        }

        public override NodeState Evaluate()
        {
            tree.currentNode = this;

            state = ClearData("wp") ? NodeState.SUCCESS : NodeState.FAILURE;

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