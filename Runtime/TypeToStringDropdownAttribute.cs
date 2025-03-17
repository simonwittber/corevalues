using System;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    public class TypeToStringDropdownAttribute : PropertyAttribute
    {
        public readonly System.Type type;
        public TypeToStringDropdownAttribute(Type type)
        {
            this.type = type;
        }
    }
}