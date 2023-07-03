using System;
using UnityEngine;

namespace Glaidiator.Model.Resources
{
    public class Health : IResource
    {
        public float Total { get; }
        public float Current { get; private set; }
        public float RegenRate { get; }
        
        public Health(float total,float regenRate)
        {
            Total = total;
            Current = Total;
            if (regenRate is < 0f or > 1.0f)
            {
                throw new ArgumentException("Regeneration rate cannot be negative or exceed 1");
            }
            RegenRate = regenRate;
        }

        public void Add(float value)
        {
            Current = Mathf.Min(Total, Current + value);
        }

        public void Subtract(float value)
        {
            Current = Mathf.Max(0, Current - value);
        }
        
        public void Regen(float deltaTime)
        {
            Add(Total * RegenRate * deltaTime);
        }

        public void Set(float value)
        {
            Current = Mathf.Clamp(value, 0, Total);
        }
    }
}