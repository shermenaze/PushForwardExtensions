
namespace PushForward.ScriptableObjects
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ScriptableObjects/Game Event", order = 20)]
    public class GameEvent : ScriptableObject
    {
        private readonly List<GameEventListener> listeners = new List<GameEventListener>();

        [ContextMenu("Raise")]
        public void Raise()
        {
            for (int listenerIndex = this.listeners.Count - 1; listenerIndex >= 0; listenerIndex--)
            { this.listeners[listenerIndex].OnEventRaised(); }
        }

        public void RegisterListener(GameEventListener listener)
        { this.listeners.Add(listener); }

        public void UnregisterListener(GameEventListener listener)
        { this.listeners.Remove(listener); }
    }
}