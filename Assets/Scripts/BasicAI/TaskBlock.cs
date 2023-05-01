using BehaviorTree;

namespace BasicAI
{
    public class TaskBlock : Node
    {
        public TaskBlock()
        {
        }

        public override NodeState Evaluate()
        {
            tree.Block = true;

            state = NodeState.SUCCESS;
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