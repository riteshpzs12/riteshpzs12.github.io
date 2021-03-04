using SensorData.Models;
using SensorData.Services;
using SensorData.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SensorData
{
    public partial class App : Application
    {
        public static string DeviceId = null;
        public ICache cache;
        public static ISensorService sensorService;
        TriggeredOptimizer triggered;
        public App()
        {
            InitializeComponent();
            //Opens the First Page, typically the Login page now
            //TODO : Will change this to registration page
            //TODO : Based on cached details the landing page will vary between registration and Login
            cache = new CacheImpl();
            sensorService = new SensorService();
            triggered = new TriggeredOptimizer(cache);
            CheckCacheAndLoad();
        }

        /// <summary>
        /// Checks and loads the proper landing page
        /// </summary>
        private async void CheckCacheAndLoad()
        {
            var cred = cache.Get<CredModel>(Config.CredCacheKey);
            var token = await Xamarin.Essentials.SecureStorage.GetAsync(Config.InstallationToken);
            if (cred != null)
            {
                //Load login screen with the credential filled, auto popup biometrics if enabled
                MainPage = new NavigationPage(new FirstPage(cred));
            }
            else
            {
                if (token == null)
                {
                    //Open the Register Page
                    MainPage = new NavigationPage(new Registration());
                }
                else
                {
                    MainPage = new NavigationPage(new FirstPage(null));
                }
            }
        }

        /// <summary>
        /// starts the triggered jobs and and data capturing
        /// </summary>
        protected override void OnStart()
        {
            //start capturing the data and check not sent data
            //prepare to send it to the server
            sensorService.StartCapture();
            triggered.Start();
        }

        /// <summary>
        /// Stops the Data capture process and the triggered job
        /// </summary>
        protected override void OnSleep()
        {
            //stop capturing and add the data into the cache
            //Stop Capturing and save the data here. Test is successfull
            //Kill the scheduled thread to save the data in a
            sensorService.DisposeAll();
            triggered.Stop();
        }

        /// <summary>
        /// Logic when app reopens
        /// </summary>
        protected override void OnResume()
        {
            sensorService.StartCapture();
            triggered.Start();
        }
    }
}
