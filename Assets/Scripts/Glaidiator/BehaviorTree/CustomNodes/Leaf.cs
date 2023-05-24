using System.Collections.Generic;
using Glaidiator.BehaviorTree.Base;

namespace Glaidiator.BehaviorTree.CustomNodes
{
    public abstract class Leaf : Node
    {
        public override void Flatten(List<Node> nodes)
        {
            nodes.Add(this);
        }
    }
}