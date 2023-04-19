﻿namespace Glaidiator.Model.Resources
{
    public interface IResource
    {
        public float Total { get; }
        public float Current { get; }
        public void Add(float value);
        public void Subtract(float value);
    }
}