using System.Xml;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Model;
using Glaidiator.Utils;
using UnityEngine;

namespace Glaidiator.BehaviorTree.CustomNodes.ConditionNodes
{
    public class ConditionCompareHealth : Condition
    {
        private float _ratio;
        public ConditionCompareHealth(float ratio)
        {
            _ratio = ratio;
        }

        public override NodeState Evaluate()
        {
            tree.currentNode = this;
            
            var enemy = (Character)GetData("enemy");
            if (enemy == null)
            {
                state = NodeState.FAILURE;
                return state;
            }

            float percentRatio = MathStuff.GetPercentDifference(tree.Owner.Health.Current, enemy.Health.Current); 
            state = percentRatio > _ratio ? NodeState.SUCCESS : NodeState.FAILURE;
            return state;
        }

        public override Node Clone()
        {
            return new ConditionCompareHealth(_ratio);
        }

        public override void Mutate()
        {
            _ratio += MathStuff.Rand.NextFloat(10f) - 5f;
            _ratio = Mathf.Clamp(_ratio, -200f, 200f);
        }

        public override Node Randomized()
        {
            return new ConditionCompareHealth(MathStuff.Rand.NextFloat(-200f, 200f));
        }
        
        public override void WriteXml(XmlWriter w)
        {
            w.WriteStartElement(GetType().Name);
            w.WriteAttributeString("ratio", _ratio.ToString());
            w.WriteEndElement();
        }
    }
}