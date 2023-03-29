﻿using UnityEngine;

namespace Glaidiator.Model
{
    public class Timer
    {
        private readonly float _duration = 0f;
        public float Duration { get; private set; }

        public Timer(float duration)
        {
            _duration = duration;
            Reset();
        }
        /**
         * Returns TRUE if timer is ticking, false if not.
         */
        public bool Tick(float deltaTime)
        {
            Duration = Mathf.Max(Duration - deltaTime, 0f);
            return Duration > 0f;
        }

        public void Reset()
        {
            Duration = _duration;
        }
    }
}