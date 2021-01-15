using SensorData.ContainerHelper;
using SensorData.Models;
using SensorData.ViewModel.SensorPage;
using Xamarin.Forms;

namespace SensorData.Views
{
    public partial class SensorPage : ContentPage
    {
        public SensorPage(MasterDataModel masterDataModel = null)
        {
            InitializeComponent();
            IIoCContainer container = Builder.Build();
            ViewModel = container.Resolve<IBaseViewModel>() as SensorPageViewModel;
            NavigationPage.SetHasNavigationBar(this, false);
            if (masterDataModel != null)
            {
                ViewModel.SensorData = masterDataModel;
            }
        }

        public SensorPageViewModel ViewModel
        {
            get { return BindingContext as SensorPageViewModel; }
            set { BindingContext = value; }
        }
    }
}
