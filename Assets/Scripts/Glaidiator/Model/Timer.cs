using UnityEngine;

namespace Glaidiator.Model
{
    public class Timer
    {
        private float _duration = 0f;
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
            _duration = Mathf.Max(_duration - deltaTime, 0f);
            return _duration > 0f;
        }

        public void Reset()
        {
            Duration = _duration;
        }
    }
}