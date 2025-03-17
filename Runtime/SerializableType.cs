using System;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    [Serializable]
    public class SerializableType
    {
        [SerializeField] public string typeName;

        private Type _type;

        public Type Type => _type ??= typeName == null? null : Type.GetType(typeName);
        
        public SerializableType(Type type)
        {
            typeName = type?.AssemblyQualifiedName;
        }
    }
    
    [Serializable]
    public class SelectableType
    {
        [SerializeReference] public SerializableType baseType;
        [SerializeReference] public SerializableType selectedType;
    }
}
