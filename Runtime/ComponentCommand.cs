using System;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    [MenuPath("Transform/Set Position")]
    [Serializable]
    public class SetPosition : IGameObjectCommand
    {
        public Vector3 position;

        public void Invoke(GameObject target)
        {
            target.transform.position = position;
        }
    }
}