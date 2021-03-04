using System;
using SensorData.ContainerHelper;
using SensorData.Services;

namespace SensorData.ViewModel.Registrations
{
    public static class Builder
    {
        public static IIoCContainer Build()
        {
            IIoCContainer container = new IoCContainer();
            container.Register<IBaseViewModel, RegistrationViewModel>();
            container.Register<INavService, NavService>();
            container.Register<IFileOperation, FileOperation>();
            container.Register<ICache, CacheImpl>();
            container.Register<IWebHelper, WebHelper>();
            return container;
        }
    }
}
