using SensorData.ContainerHelper;
using SensorData.Services;
using SensorData.ViewModel.FirstPage;

namespace SensorData.Views
{
    public static class Builder
    {
        public static IIoCContainer Build()
        {
            IIoCContainer container = new IoCContainer();
            container.Register<IBaseViewModel, FirstPageViewModel>();
            container.Register<ISensorService, SensorService>();
            return container;
        }
    }
}
