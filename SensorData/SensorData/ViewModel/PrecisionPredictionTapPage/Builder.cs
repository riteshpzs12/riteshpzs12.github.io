using SensorData.ContainerHelper;
using SensorData.Services;

namespace SensorData.ViewModel.PrecisionPredictionTapPage
{
    public static class Builder
    {
        public static IIoCContainer Build()
        {
            IIoCContainer container = new IoCContainer();
            container.Register<IBaseViewModel, PrecisionPredictionTapPageViewModel>();
            //container.Register<ISensorService, SensorService>();
            container.Register<INavService, NavService>();
            return container;
        }
    }
}
