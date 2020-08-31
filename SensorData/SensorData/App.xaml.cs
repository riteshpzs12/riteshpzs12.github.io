using System;
using SensorData.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SensorData
{
    public partial class App : Application
    {
        public static string DeviceId = null;

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new FirstPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
