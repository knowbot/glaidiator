using UnityEngine;

namespace Glaidiator.Model.Resources
{
    public class Health : IResource
    {
        public float Total { get; }
        public float Current { get; private set; }
        
        public Health(float total)
        {
            Total = total;
            Current = Total;
        }

        public void Add(float value)
        {
            Current = Mathf.Max(Total, Current + value);
        }

        public void Subtract(float value)
        {
            Current = Mathf.Max(0, Current - value);
        }
    }
}