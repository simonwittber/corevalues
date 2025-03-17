using System;

namespace Dffrnt.CoreValues
{
    [AttributeUsage(AttributeTargets.Struct)]
    public class CoreValueTypeAttribute : Attribute
    {
        public readonly Type type;

        public CoreValueTypeAttribute(Type type)
        {
            this.type = type;
        }
    }
}