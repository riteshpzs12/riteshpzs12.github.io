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

            //Opens the Firs Page, typically the Login page now
            //TODO : Will change this to registration page
            //TODO : Based on cached details the landing page will vary between registration and Login
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
