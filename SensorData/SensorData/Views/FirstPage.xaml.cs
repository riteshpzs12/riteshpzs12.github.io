using SensorData.ContainerHelper;
using SensorData.Models;
using SensorData.ViewModel.FirstPage;
using Xamarin.Forms;

namespace SensorData.Views
{
    public partial class FirstPage : ContentPage
    {
        public FirstPage(CredModel cred = null)
        {
            InitializeComponent();
            IIoCContainer container = Builder.Build();
            ViewModel = container.Resolve<IBaseViewModel>() as FirstPageViewModel;
            NavigationPage.SetHasNavigationBar(this, false);
            if(cred!=null)
            {
                ViewModel.FillDetails(cred);
            }
        }

        public FirstPageViewModel ViewModel
        {
            get { return BindingContext as FirstPageViewModel; }
            set { BindingContext = value; }
        }

        //Not needed
        /// <summary>
        /// when tap on entry field starts capturing data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //void Entry_Focused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        //{
        //    ViewModel.StartCapture();
        //}

        //Not needed
        /// <summary>
        /// Sensor instances are disposed
        /// </summary>
        //protected override void OnDisappearing()
        //{
        //    base.OnDisappearing();
        //    ViewModel.DisposeSubscribers();
        //}

        ///Not needed
        /// <summary>
        /// Sets the initial state for the page
        /// </summary>
        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    Username.Placeholder = "Enter Ur Name";
        //    Password.Placeholder = "Enter Sensor@123 as Password";
        //    ViewModel.StartOver();
        //}
    }
}
