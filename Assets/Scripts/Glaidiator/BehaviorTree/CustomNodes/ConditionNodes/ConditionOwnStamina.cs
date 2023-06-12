using System.Xml;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Utils;
using UnityEngine;

namespace Glaidiator.BehaviorTree.CustomNodes.ConditionNodes
{
    public class ConditionOwnStamina: Condition
    {
        
        
        private float _threshold;
        public ConditionOwnStamina(float threshold)
        {
            _threshold = threshold;
        }

        public override NodeState Evaluate()
        {
            tree.currentNode = this;

            if (tree.Owner.Stamina.Current >= _threshold)
            {
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }

        public override Node Clone()
        {
            return new ConditionOwnStamina(_threshold);
        }

        public override void Mutate()
        {
            _threshold += MathStuff.Rand.NextFloat(10f) - 5f;
            _threshold = Mathf.Clamp(_threshold, 0f, 100f);
        }

        public override Node Randomized()
        {
            return new ConditionOwnStamina(MathStuff.Rand.NextFloat(100f));
        }
        public override void WriteXml(XmlWriter w)
        {
            w.WriteStartElement(GetType().Name);
            w.WriteAttributeString("threshold", _threshold.ToString());
            w.WriteEndElement();
        }
    }
}