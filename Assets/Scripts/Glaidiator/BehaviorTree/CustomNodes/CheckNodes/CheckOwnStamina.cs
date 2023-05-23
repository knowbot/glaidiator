using System;
using System.Xml;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Utils;
using UnityEngine;

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
            _threshold += MathStuff.Rand.NextFloat(10f) - 5f;
            _threshold = Mathf.Clamp(_threshold, 0f, 100f);
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