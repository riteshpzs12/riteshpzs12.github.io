using System;
using SensorData.ContainerHelper;
using SensorData.ShinySensor;
using SensorData.ViewModel.FirstPage;
using Shiny;

namespace SensorData.Views
{
    public static class Builder
    {
        public static IIoCContainer Build()
        {
            IIoCContainer container = new IoCContainer();
            container.Register<IBaseViewModel, FirstPageViewModel>();
            container.Register<ShinyStartup, SensorStartup>();
            return container;
        }
    }
}
