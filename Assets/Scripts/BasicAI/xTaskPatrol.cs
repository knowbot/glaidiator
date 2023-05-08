using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Glaidiator.Model;

namespace BasicAI
{
    public class xTaskPatrol : Node // deprecated
    {
        private Movement _transform;
        private Vector2 _currPos;
        private List<Vector2> _waypoints;
        
        private int _currentWP;
        private float _waitTime = 1f;
        private float _waitCounter = 0f;
        private bool _isWaiting = false;


        public xTaskPatrol(BTree btree, Character myCharacter)
        {
            tree = btree;
            _ownerCharacter = myCharacter;
            _transform = myCharacter.Movement;
            _currPos = _transform.Position.xz();
            _waypoints = new List<Vector2>();
            _waypoints.Add(_currPos + new Vector2(0f, 0f));
            _waypoints.Add(_currPos + new Vector2(2f, 0f));
            _waypoints.Add(_currPos + new Vector2(2f, 2f));
            _waypoints.Add(_currPos + new Vector2(0f, 2f));
        }

        public override NodeState Evaluate()
        {
            tree.currentNode = this;
            if (_isWaiting)
            {
                tree.Direction = _transform.LastDir;
                tree.Move = false;
                
                _waitCounter += Time.deltaTime;
                if (_waitCounter >= _waitTime)
                {
                    _isWaiting = false;
                }
            }
            else
            {
                _currPos = _transform.Position.xz();
                Vector2 wp = _waypoints[_currentWP];

                if (Vector2.Distance(_currPos, wp) < 0.01f)
                {
                    tree.Direction = _transform.LastDir;
                    tree.Move = false;
                    
                    _currPos = wp;
                    _waitCounter = 0f;
                    _isWaiting = true;

                    _currentWP = (_currentWP + 1) % _waypoints.Count;
                    //Debug.Log("current WP is " + _currentWP);
                    
                }
                else
                {
                    tree.Direction = (wp - _currPos).normalized.x0y();
                    tree.Move = true;
                    
                    Vector3 temp3d = _currPos.x0y();
                    Vector3 wp3d = wp.x0y();
                    Debug.DrawLine(temp3d, wp3d, Color.blue);
                }

            }

            
            state = NodeState.RUNNING;
            return state;
        }

        public override Node Clone()
        {
            return new xTaskPatrol(tree, _ownerCharacter);
        }

        public override void Mutate()
        {
            throw new System.NotImplementedException();
        }

        public override Node Randomized()
        {
            throw new System.NotImplementedException();
        }
    }

}



