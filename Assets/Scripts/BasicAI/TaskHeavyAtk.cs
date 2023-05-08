using BehaviorTree;

namespace BasicAI
{
    public class TaskHeavyAtk : Node
    {

        public TaskHeavyAtk()
        {
        }

        public override NodeState Evaluate()
        {
            tree.currentNode = this;// for debug info
            tree.AttackHeavy = true;

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