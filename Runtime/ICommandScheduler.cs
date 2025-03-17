using UnityEngine;

namespace Dffrnt.CoreValues
{
    public interface ICommandScheduler : IServiceInterface
    {
        void Schedule(IGameObjectCommand command, GameObject gameObject);
    }
}