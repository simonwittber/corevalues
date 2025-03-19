using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    [NiceName("Command List")]
    [MenuPath("Common")]
    [Serializable]
    public class CommandList : IGameObjectCommand
    {
        [SerializeReference, RefPicker]
        public List<IGameObjectCommand> commands;

        public void Invoke(GameObject target)
        {
            foreach(var i in commands)
                i.Invoke(target);
        }
    }
}