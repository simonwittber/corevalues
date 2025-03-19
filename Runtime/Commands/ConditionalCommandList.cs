using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    [NiceName("Conditional Command List")]
    [MenuPath("Common")]
    [Serializable]
    public class ConditionalCommandList : IGameObjectCommand
    {
        [SerializeReference, RefPicker]
        public IGameObjectCondition condition;
        
        [SerializeReference, RefPicker]
        public List<IGameObjectCommand> ifTrue;
        [SerializeReference, RefPicker]
        public List<IGameObjectCommand> ifFalse;

        public void Invoke(GameObject target)
        {
            if (!condition.Invoke(target))
            {
                foreach (var i in ifTrue)
                    i.Invoke(target);
            }
            else
            {
                foreach (var i in ifFalse)
                    i.Invoke(target);
            }
        }
        
    }
}