using UnityEngine;

namespace Dffrnt.CoreValues
{
    public interface IGameObjectCondition
    {
        bool Invoke(GameObject target);
    }
}