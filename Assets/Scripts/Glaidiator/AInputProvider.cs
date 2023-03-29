using UnityEngine;

namespace Glaidiator
{
    public abstract class AInputProvider : MonoBehaviour
    {
        protected Input Inputs;
        public abstract Input GetInputs();
    }
}