
namespace PushForward.ScriptableObjects.Primitives
{
    using System;

    [Serializable]
    public class FloatReference
    {
        public bool useInitial = true;
        public FloatVariable variable;

        public float Value => this.useInitial ? this.variable.initialValue : this.variable.runtimeValue;
    }
}
