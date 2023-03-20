namespace Character.Model
{
    public class Lock
    {
        private float _delay = 0f;
        private float _duration = 0f;

        public Lock(float delay, float duration)
        {
            _delay = delay;
            _duration = duration;
        }
        
        public bool Tick(float deltaTime)
        {
            if (_delay > 0f)
            {
                _delay -= deltaTime;
                return false;
            }
            if (_duration > 0f)
            {
                _duration -= deltaTime;
                return true;
            }
            return false;
        }

        public void SetLock(float delay, float duration)
        {
            _delay = delay;
            _duration = duration;
        }
    }
}