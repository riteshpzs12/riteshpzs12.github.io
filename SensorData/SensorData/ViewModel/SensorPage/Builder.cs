using SensorData.ContainerHelper;
using SensorData.Services;

namespace SensorData.ViewModel.SensorPage
{
    public static class Builder
    {
        public static IIoCContainer Build()
        {
            IIoCContainer container = new IoCContainer();
            container.Register<IBaseViewModel, SensorPageViewModel>();
            container.Register<INavService, NavService>();
            return container;
        }
    }
}
