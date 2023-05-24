using System;
using System.Xml;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Model;
using Glaidiator.Utils;
using UnityEngine;

namespace Glaidiator.BehaviorTree.CustomNodes.CheckNodes
{
    public class CheckEnemyDistance: Check
    {
        private float _distance;
        public CheckEnemyDistance(float distance)
        {
            _distance = distance;
        }


        public override NodeState Evaluate()
        {
            tree.currentNode = this;// for debug info
            if (tree == null) throw new NullReferenceException();
        
            Movement target = ((Character)GetData("enemy"))?.Movement;
            if (target == null)
            {
                ////Debug.Log("CheckEnemyDistance target = null");
                state = NodeState.FAILURE;
                return state;
            }

            float dist = Vector3.Distance(tree.Owner.Movement.Position, target.Position);
            tree.enemyDistance = dist;

            if (dist <= _distance)
            {
                state = NodeState.SUCCESS;
                //Debug.DrawLine(_tree.OwnerCharacter.Movement.Position, target.Position, Color.green, 0.1f);
                ////Debug.Log("CheckEnemyDistance state = "+state);
            }
            else
            {
                state = NodeState.FAILURE;
                //Debug.DrawLine(_tree.OwnerCharacter.Movement.Position, target.Position, Color.red, 0.1f);
            }
            
            return state;
        }
        
        
        public override Node Clone()
        {
            return new CheckEnemyDistance(_distance);
        }

        public override void Mutate()
        {
            _distance += MathStuff.Rand.NextFloat(4f) - 2f;
            _distance = Mathf.Clamp(_distance, 0f, Arena.Diagonal);
        }

        public override Node Randomized()
        {
            return new CheckEnemyDistance(MathStuff.Rand.NextFloat(Arena.Diagonal));
        }
        public override void WriteXml(XmlWriter w)
        {
            w.WriteStartElement(GetType().Name);
            w.WriteAttributeString("distance", _distance.ToString());
            w.WriteEndElement();
        }

        
    }
}