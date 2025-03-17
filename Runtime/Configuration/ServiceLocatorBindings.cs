
namespace Dffrnt.CoreValues
{
    public static class ServiceLocatorBindings
    {
        public static void BindAll()
        {
             ServiceLocator<Dffrnt.CoreValues.IAnotherDemoService>.Bind(new Dffrnt.CoreValues.AnotherDemoService());
             ServiceLocator<Dffrnt.CoreValues.ICommandScheduler>.Bind(new Dffrnt.CoreValues.RuntimeCommandScheduler());
             ServiceLocator<Dffrnt.CoreValues.IDemoxxxService>.Bind(new Dffrnt.CoreValues.AltDemoService());

        }
    }
}
