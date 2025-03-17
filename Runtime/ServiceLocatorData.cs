using System;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    [Serializable]
    public class ServiceBindingPair
    {
        [SerializeReference] public SerializableType interfaceType;
        [SerializeReference] public SelectableType implementationType;

        public override string ToString()
        {
            return $"{interfaceType.Type} -> {implementationType.baseType.Type}";
        }
    }
    
    [CreateAssetMenu(menuName = "Core/Service Locator Data")]
    public class ServiceLocatorData : ScriptableObject
    {
        public ServiceBindingPair[] bindings;
    }
}