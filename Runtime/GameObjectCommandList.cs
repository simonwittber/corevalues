using UnityEngine;

namespace Dffrnt.CoreValues
{
    [MenuPath("GameObjects/Command List")]
    public class GameObjectCommandList : IGameObjectCommand
    {
        [SerializeReference] [RefPicker] private IGameObjectCommand[] commands;
        
        public virtual void Invoke(GameObject gameObject)
        {
            foreach (var i in commands) 
                i?.Invoke(gameObject);
        }
    }
}