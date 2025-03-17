namespace Dffrnt.CoreValues
{
    public interface IServiceInterface {}

    public interface IServiceImplementation {}
    
    public interface IServiceImplementation<T> : IServiceImplementation
    {
    }

    public interface IDemoxxxService : IServiceInterface
    {
        void DoSomething();
    }
    
    public class DemoxxxService : IDemoxxxService, IServiceImplementation<IDemoxxxService>
    {
        public void DoSomething()
        {
            UnityEngine.Debug.Log("Doing something");
        }
    }
    
    public class AltDemoService : IDemoxxxService, IServiceImplementation<IDemoxxxService>
    {
        public void DoSomething()
        {
            UnityEngine.Debug.Log("Doing something");
        }
    }

    
    public interface IAnotherDemoService : IServiceInterface
    {
        void DoSomethingElse();
    }
    
    public class AnotherDemoService : IAnotherDemoService, IServiceImplementation<IAnotherDemoService>
    {
        public void DoSomethingElse()
        {
            UnityEngine.Debug.Log("Doing something else");
        }
    }
}
