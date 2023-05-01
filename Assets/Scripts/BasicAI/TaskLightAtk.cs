using BehaviorTree;

namespace BasicAI
{
    public class TaskLightAtk : Node
    {
        public TaskLightAtk()
        {
            
        }

        public override NodeState Evaluate()
        {
            tree.AttackLight = true;

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