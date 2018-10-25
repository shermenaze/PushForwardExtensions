namespace PushForward.ScriptableObjects.Runtime
{
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class RuntimeSet<T> : ScriptableObject
    {
        public List<T> items = new List<T>();

        public void Add(T t)
        {
            if (!this.items.Contains(t))
            { this.items.Add(t);}
        }

        public void Remove(T t)
        {
            if (this.items.Contains(t))
            { this.items.Remove(t); }
        }
    }
}