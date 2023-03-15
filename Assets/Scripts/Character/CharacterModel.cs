using System;
using UnityEngine;

namespace Character
{
    public class CharacterModel : MonoBehaviour
    {
        private CharacterPresenter _presenter;
        private CharacterMovement _movement;

        private void Awake()
        {
            _presenter = GetComponent<CharacterPresenter>();
        }

        private void Update()
        {
            transform.position += _movement.Move(_presenter.InputMove, Time.deltaTime);
        }
    }
}