using System;

namespace Dffrnt.CoreValues
{
    public static class ServiceLocator<TA> where TA : class
    {
        static System.Func<TA> constructor = null;
        static System.Type type;

        public static void Bind<TB> () where TB:TA,new()
        {
            type = typeof(TB);
            constructor = () => new TB ();
        }

        public static void Bind<TB> (TB instance) where TB : TA
        {
            type = instance.GetType ();
            constructor = () => instance;
        }

        public static TA Resolve ()
        {
            if (constructor == null)
                throw new UnboundInterfaceException ();
            return constructor ();
        }

        public static System.Type ResolveType ()
        {
            if (type == null)
                throw new UnboundInterfaceException ();
            return type;
        }
    }

    public class UnboundInterfaceException : Exception
    {
    }
}