using System;
using System.Collections.Generic;
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

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            App.Current.MainPage.DisplayAlert("dasda", "adsasdas", "dasdasd");
        }
    }
}
