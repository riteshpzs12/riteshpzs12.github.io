using SensorData.ContainerHelper;
using SensorData.Services;

namespace SensorData.ViewModel.FirstPage
{
    public static class Builder
    {
        public static IIoCContainer Build()
        {
            IIoCContainer container = new IoCContainer();
            container.Register<IBaseViewModel, FirstPageViewModel>();
            //container.Register<ISensorService, SensorService>();
            container.Register<INavService, NavService>();
            container.Register<ICache, CacheImpl>();
            container.Register<IWebHelper, WebHelper>();
            return container;
        }
    }
}
