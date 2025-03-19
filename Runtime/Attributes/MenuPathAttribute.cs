using System;

namespace Dffrnt.CoreValues
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class MenuPathAttribute : Attribute
    {
        public readonly string path;

        public MenuPathAttribute(string path)
        {
            this.path = path;
        }
    }
}