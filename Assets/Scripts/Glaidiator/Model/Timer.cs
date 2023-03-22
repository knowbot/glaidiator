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
        
        public void Tick(float deltaTime)
        {
            if (_delay > 0f) _delay -= deltaTime;
            if (_duration <= 0f) return;
            _duration -= deltaTime;
            if (_duration < 0f) _duration = 0f;
        }

        public void Set(float delay, float duration)
        {
            _delay = delay;
            _duration = duration;
        }

        public bool Active()
        {
            return _duration > 0;
        }
    }
}