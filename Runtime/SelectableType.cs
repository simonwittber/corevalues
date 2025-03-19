using System;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    [Serializable]
    public class SelectableType
    {
        [SerializeReference] public SerializableType baseType;
        [SerializeReference] public SerializableType selectedType;
    }
}