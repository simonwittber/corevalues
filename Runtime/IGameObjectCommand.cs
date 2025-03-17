using UnityEngine;

namespace Dffrnt.CoreValues
{
    public interface IGameObjectCommand
    {
        void Invoke(GameObject target);
    }
}