using UnityEngine;

namespace Dffrnt.CoreValues
{
    public class GameEventListener : MonoBehaviour, IInvokable
    {
        public bool repeat;
        [Tooltip("Event to register with.")] 
        [SerializeField] private GameEvent gameEvent;

        [SerializeReference, RefPicker] private IGameObjectCommand command;

        private void OnEnable()
        {
            gameEvent.Register(this);
        }

        private void OnDisable()
        {
            gameEvent.Unregister(this);
        }

        public virtual void Invoke() => command.Invoke(gameObject);
    }
}