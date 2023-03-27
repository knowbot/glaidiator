using UnityEngine;

namespace Glaidiator.Model
{
    public abstract class AInputProvider : MonoBehaviour
    {
        protected Input Inputs;
        public abstract Input GetInputs();
    }
}