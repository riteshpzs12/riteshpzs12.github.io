using SensorData.ContainerHelper;
using SensorData.ViewModel.FirstPage;
using Xamarin.Forms;

namespace SensorData.Views
{
    public partial class FirstPage : ContentPage
    {
        public FirstPage()
        {
            InitializeComponent();
            IIoCContainer container = Builder.Build();
            ViewModel = container.Resolve<IBaseViewModel>() as FirstPageViewModel;
        }

        public FirstPageViewModel ViewModel
        {
            get { return BindingContext as FirstPageViewModel; }
            set { BindingContext = value; }
        }

        void Entry_Focused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            ViewModel.StartCapture();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ViewModel.DisposeSubscribers();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Username.Placeholder = "Enter Ur Name";
            Password.Placeholder = "Enter Sensor@123 as Password";
            ViewModel.StartOver();
        }
    }
}
