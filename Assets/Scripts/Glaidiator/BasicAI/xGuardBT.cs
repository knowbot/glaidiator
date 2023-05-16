using BehaviorTree;
using Glaidiator.Model;

namespace BasicAI
{
    public class xGuardBT : BTree // deprecated
    {
        
        public static float speed = 2f;
        public static float fovRange = 4f;
        public static float attackRange = 0.5f;
        public static float patrolStep = 2f;
        public static bool isPatrolRight = true;


        protected override Node SetupTree()
        {
            //Debug.Log("Setup GuardBT");
            //_transform = transform;
            //Node root = new TaskPatrol(transform, waypoints);

            /*
            Node root = new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new CheckEnemyInRange(_transform),
                    new TaskGoToTarget(_transform),
                }),
                new TaskPatrol(_transform, waypoints),
            });
            
            Node root = new Selector(this, new List<Node>
            {
                new Sequence(this, new List<Node>
                {
                    new CheckEnemyInRange(this, _transform),
                    new TaskGoToTarget(this, _transform),
                }),
                new TaskPatrol(this, _transform),
            });
            */
            // return root;
            return null;
        }

        public override BTree Clone()
        {
            return this;
        }

        public xGuardBT(Character owner) : base(owner)
        {
        }
    }
}