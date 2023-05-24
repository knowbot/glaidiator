using System;
using System.Xml;
using Glaidiator.BehaviorTree.Base;
using Glaidiator.Model;
using Glaidiator.Utils;
using UnityEngine;

namespace Glaidiator.BehaviorTree.CustomNodes.CheckNodes
{
    public class CheckTargetDistance: Check
    {
        private float _distance;
        private string _targetName;

        public CheckTargetDistance(string targetName,float distance)
        {
            _distance = distance;
            _targetName = targetName;
        }
        
        public override NodeState Evaluate()
        {
            tree.currentNode = this;// for debug info
            var target = GetData(_targetName);
            if (target is not Vector3)
            {
                //Debug.Log("CheckDistanceToWP, no waypoint found");
                state = NodeState.FAILURE;
                return state;
            }
            
            //float signedDistance = MathStuff.GetSignedDistance(_tree.OwnerCharacter.Movement.Position, (Vector3)target, tree.Direction);
            float distance = Vector3.Distance(tree.Owner.Movement.Position, (Vector3)target);
            Vector3 nDir = ((Vector3)target - tree.Owner.Movement.Position).normalized;
            Vector3 nnDir = MathStuff.Get8DDirection(nDir.x, nDir.z);
            
            if (distance <= _distance || nnDir != tree.Direction)
            {
                ////Debug.Log("successfully reached waypoint = " + target);
                //Debug.Log(_tree.OwnerCharacter.Movement.Position);
                state = NodeState.SUCCESS;
            }
            else
            {
                ////Debug.Log("wp dist = "+distance);
                state = NodeState.FAILURE;
            }
            
            return state;
        }

        public override Node Clone()
        {
            return new CheckTargetDistance(_targetName, _distance);
        }

        public override void Mutate()
        {
            _distance += MathStuff.Rand.NextFloat(4f) - 2f;
            _distance = Mathf.Clamp(_distance, 0f, Arena.Diagonal);
        }

        public override Node Randomized()
        {
            return new CheckTargetDistance(_targetName, MathStuff.Rand.NextFloat(Arena.Diagonal));
        }
        
        public override void WriteXml(XmlWriter w)
        {
            w.WriteStartElement(GetType().Name);
            w.WriteAttributeString("distance", _distance.ToString());
            w.WriteEndElement();
        }

    }
}