using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace AutomationAnywhere.Ipc.Receiver
{
    public static class Bootstrapper
    {
        /// <summary>
        /// Initializes unity service locator and load global components
        /// </summary>
        public static void Init()
        {
            //Configure service locator
            var locator = new UnityServiceLocator(ConfigureUnityContainer());
            ServiceLocator.SetLocatorProvider(() => locator);
        }

        private static IUnityContainer ConfigureUnityContainer()
        {
            var container = new UnityContainer();
            container.LoadConfiguration();
            return container;
        }
    }
}
