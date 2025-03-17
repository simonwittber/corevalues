using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Dffrnt.CoreValues
{
    public class RuntimeCommandScheduler : ICommandScheduler
    {
        public void Schedule(IGameObjectCommand command, GameObject gameObject)
        {
            command?.Invoke(gameObject);  
        } 
    }
}