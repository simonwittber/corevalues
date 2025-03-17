#nullable disable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    [CreateAssetMenu]
    public class GameEvent : ScriptableObject, IInvokable
    {
        private readonly HashSet<IInvokable> _listeners = new();

        public void Invoke()
        {
            OnInvoke?.Invoke();
            foreach (var listener in _listeners)
                listener.Invoke();
        }

        public event Action OnInvoke;

        public void Register(IInvokable listener)
        {
            _listeners.Add(listener);
        }

        public void Unregister(IInvokable listener)
        {
            _listeners.Remove(listener);
        }

        public static void Trigger(IInvokable ev)
        {
            ev?.Invoke();
        }
    }
}