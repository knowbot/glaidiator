using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

namespace BasicAI
{
    public class TaskPatrol : Node
    {
        private Transform _transform; // curr pos of this agent
        //private Transform[] _waypoints;
        //private Animator _animator;

        private Vector3 _currPos;
        private List<Vector3> _waypoints;


        private int _currentWP;
        private float _waitTime = 1f;
        private float _waitCounter = 0f;
        private bool _isWaiting = false;

        //private float _speed = 2f;

        public TaskPatrol(Transform transform, List<Vector3> waypoints)
        {
            _transform = transform;
            _currPos = _transform.position;
            _waypoints = waypoints;
            //_animator = transform.GetComponent<Animator>();

            _waypoints.Add(_currPos + new Vector3(0f, 0f, 0f));
            _waypoints.Add(_currPos + new Vector3(2f, 0f, 0f));
            _waypoints.Add(_currPos + new Vector3(2f, 0f, 2f));
            _waypoints.Add(_currPos + new Vector3(0f, 0f, 2f));

        }

        public override NodeState Evaluate()
        {
            

            if (_isWaiting)
            {
                _waitCounter += Time.deltaTime;
                if (_waitCounter >= _waitTime)
                {
                    _isWaiting = false;
                    //_animator.SetBool("Walking", true);
                }

            }
            else
            {
                //var currPos = _transform.position;
                //Transform wp = _waypoints[_currentWP];
                Vector3 wp = _waypoints[_currentWP];

                if (Vector3.Distance(_transform.position, wp) < 0.01f)
                {

                    _transform.position = wp;
                    _waitCounter = 0f;
                    _isWaiting = true;

                    _currentWP = (_currentWP + 1) % _waypoints.Count;
                    Debug.Log("current WP is " + _currentWP);

                    //_animator.SetBool("Walking", false);
                }
                else
                {
                    _transform.position = Vector3.MoveTowards(_transform.position,
                        wp,
                        GuardBT.speed * Time.deltaTime);
                    _transform.LookAt(wp);
                    Debug.DrawLine(_transform.position, wp, Color.blue);
                }

            }

            state = NodeState.RUNNING;
            return state;
        }
    }

}



