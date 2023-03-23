using UnityEngine;

namespace Glaidiator.Model
{
    public class Timer
    {
        private float _delay = 0f;
        private float _duration = 0f;

        public Timer(float delay, float duration)
        {
            _delay = delay;
            _duration = duration;
        }
        
        public bool Tick(float deltaTime)
        {
            _delay = Mathf.Max(_delay - deltaTime, 0f);
            if (_delay > 0f)
                return false;
            _duration = Mathf.Max(_duration - deltaTime, 0f);
            return _duration == 0f;
        }

        public void Set(float delay, float duration)
        {
            _delay = delay;
            _duration = duration;
        }
    }
}