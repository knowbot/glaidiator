using System.Xml;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Model;
using Glaidiator.Utils;
using UnityEngine;

namespace Glaidiator.BehaviorTree.CustomNodes.ConditionNodes
{
    public class ConditionEnemyHealth: Condition
    {
        private float _threshold;

        public ConditionEnemyHealth(float threshold)
        {
            _threshold = threshold;
        }

        public override NodeState Evaluate()
        {
            tree.Active = this;

            Character enemy = (Character)GetData("enemy");
            if (enemy == null)
            {
                //Debug.Log("CheckEnemyHealth enemy = null");
                state = NodeState.FAILURE;
                return state;
            }

            float enemyHealth = enemy.Health.Current;
            state = enemyHealth >= _threshold ? NodeState.SUCCESS : NodeState.FAILURE;
            
            return state;
        }

        public override Node Clone()
        {
            return new ConditionEnemyHealth(_threshold);
        }

        public override void Mutate()
        {
            _threshold += MathStuff.Rand.NextFloat(10f) - 5f;
            _threshold = Mathf.Clamp(_threshold, 0f, 100f);
        }

        public override Node Randomized()
        {
            return new ConditionEnemyHealth(MathStuff.Rand.NextFloat(100f));
        }
        
        public override void WriteXml(XmlWriter w)
        {
            w.WriteStartElement(GetType().Name);
            w.WriteAttributeString("threshold", _threshold.ToString());
            w.WriteEndElement();
        }
    }
}