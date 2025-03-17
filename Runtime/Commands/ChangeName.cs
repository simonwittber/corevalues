using System;
using Unity.Properties;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    [MenuPath("GameObject/Change Name")]
    [Serializable]
    public class ChangeName : IGameObjectCommand
    {
        public string name;

        public void Invoke(GameObject target)
        {
            target.name = name;
        }
        
    }
}