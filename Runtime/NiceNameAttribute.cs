using System;

namespace Dffrnt.CoreValues
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class NiceNameAttribute : Attribute
    {
        public readonly string name;

        public NiceNameAttribute(string name)
        {
            this.name = name;
        }
    }
}