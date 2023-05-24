using System;
using System.Xml;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Model;
using Glaidiator.Utils;
using UnityEngine;

namespace Glaidiator.BehaviorTree.CustomNodes.CheckNodes
{
    public class CheckEnemyStamina: Check
    {
        private float _threshold;
        public CheckEnemyStamina(float threshold)
        {
            _threshold = threshold;
        }


        public override NodeState Evaluate()
        {
            tree.currentNode = this;

            Character enemy = (Character)GetData("enemy");
            if (enemy == null)
            {
                //Debug.Log("CheckEnemyStamina enemy = null");
                state = NodeState.FAILURE;
                return state;
            }

            float enemyStamina = enemy.Stamina.Current;
            if (enemyStamina >= _threshold)
            {
                state = NodeState.SUCCESS;
            }
            else
            {
                state = NodeState.FAILURE;
            }
            
            return state;
        }

        
        public override Node Clone()
        {
            return new CheckEnemyStamina(_threshold);
        }

        public override void Mutate()
        {
            _threshold += MathStuff.Rand.NextFloat(10f) - 5f;
            _threshold = Mathf.Clamp(_threshold, 0f, 100f);
        }

        public override Node Randomized()
        {
            return new CheckEnemyStamina(MathStuff.Rand.NextFloat(100f));
        }
        
        public override void WriteXml(XmlWriter w)
        {
            w.WriteStartElement(GetType().Name);
            w.WriteAttributeString("threshold", _threshold.ToString());
            w.WriteEndElement();
        }
    }
}