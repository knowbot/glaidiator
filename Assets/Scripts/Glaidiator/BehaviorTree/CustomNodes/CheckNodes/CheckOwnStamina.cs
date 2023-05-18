using System;
using System.Xml;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Utils;

namespace Glaidiator.BehaviorTree.CustomNodes.CheckNodes
{
    public class CheckOwnStamina: Check
    {
        
        
        private float _threshold;
        public CheckOwnStamina(float threshold)
        {
            _threshold = threshold;
        }

        public override NodeState Evaluate()
        {
            tree.currentNode = this;

            if (owner.Stamina.Current >= _threshold)
            {
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }

        public override Node Clone()
        {
            return new CheckOwnStamina(_threshold);
        }

        public override void Mutate()
        {
            throw new NotImplementedException();
        }

        public override Node Randomized()
        {
            return new CheckOwnStamina(MathStuff.Rand.NextFloat(100f));
        }
        public override void WriteXml(XmlWriter w)
        {
            w.WriteStartElement(GetType().Name);
            w.WriteAttributeString("threshold", _threshold.ToString());
            w.WriteEndElement();
        }
    }
}