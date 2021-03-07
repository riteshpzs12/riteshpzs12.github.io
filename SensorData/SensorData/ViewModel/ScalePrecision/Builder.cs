using System;
using SensorData.ContainerHelper;
using SensorData.Services;

namespace SensorData.ViewModel.ScalePrecision
{
    public static class Builder
    {
        public static IIoCContainer Build()
        {
            IIoCContainer container = new IoCContainer();
            container.Register<IBaseViewModel, ScalePrecisionViewModel>();
            container.Register<INavService, NavService>();
            return container;
        }
    }
}
