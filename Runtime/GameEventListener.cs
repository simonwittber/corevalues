using UnityEngine;

namespace Dffrnt.Core
{
    public class GameEventListener : MonoBehaviour, IInvokable
    {
        [Tooltip("Event to register with.")] public GameEvent ev;

        private void OnEnable() => ev.Register((IInvokable) this);

        private void OnDisable() => ev.Unregister((IInvokable) this);

        public virtual void Invoke()
        {
        }
    }
}