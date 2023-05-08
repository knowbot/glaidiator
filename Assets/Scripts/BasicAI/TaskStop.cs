using BehaviorTree;

namespace BasicAI
{
    public class TaskStop : Node
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