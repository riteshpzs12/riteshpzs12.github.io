using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using SensorData.ContainerHelper;
using SensorData.Views;
using Shiny;
using Shiny.Sensors;
using Xamarin.Forms;

namespace SensorData.ViewModel.FirstPage
{
    public class FirstPageViewModel : BaseViewModel
    {
        Dictionary<long, CompassReading> compass = new Dictionary<long, CompassReading>();
        Dictionary<long, MotionReading> accelerometer = new Dictionary<long, MotionReading>();
        Dictionary<long, MotionReading> gyroscope = new Dictionary<long, MotionReading>();
        Dictionary<long, bool> proximity = new Dictionary<long, bool>();
        Dictionary<long, ushort> heartRate = new Dictionary<long, ushort>();
        ICompass Compass;
        IAccelerometer Accelerometer;
        IGyroscope GyroScope;
        IProximity Proximity;
        IHeartRateMonitor HeartRate;
        List<IDisposable> availableSensors;
        public FirstPageViewModel()
        {
            LoginCommand = new Command(Login);
            FingerPrintCommand = new Command(FingerPrintLoginAsync);
            ResolveAllSensors();
        }

        private async void FingerPrintLoginAsync(object obj)
        {
            StartCapture();
            if (await CrossFingerprint.Current.IsAvailableAsync(false))
            {
                AuthenticationRequestConfiguration config = new AuthenticationRequestConfiguration("Wait","Let me check who are u");
                if((await CrossFingerprint.Current.AuthenticateAsync(config)).Authenticated)
                {
                    PassWord = "9Ib_ha@Z@q";
                    Login();
                }
            }
        }

        private void ResolveAllSensors()
        {
            Compass = ShinyHost.Resolve<ICompass>();
            Accelerometer = ShinyHost.Resolve<IAccelerometer>();
            GyroScope = ShinyHost.Resolve<IGyroscope>();
            Proximity = ShinyHost.Resolve<IProximity>();
            HeartRate = ShinyHost.Resolve<IHeartRateMonitor>();
        }

        internal void StartCapture()
        {
            if(Compass != null)
            {
                availableSensors.Add(Compass.WhenReadingTaken().Subscribe(c =>
                {
                    compass.Add(DateTime.UtcNow.Ticks, c);
                }));
            }

            if (Accelerometer != null)
            {
                availableSensors.Add(Accelerometer.WhenReadingTaken().Subscribe(a =>
                {
                    accelerometer.Add(DateTime.UtcNow.Ticks, a);
                }));
            }

            if (GyroScope != null)
            {
                availableSensors.Add(GyroScope.WhenReadingTaken().Subscribe(g =>
                {
                    gyroscope.Add(DateTime.UtcNow.Ticks, g);
                }));
            }

            if (Proximity != null)
            {
                availableSensors.Add(Proximity.WhenReadingTaken().Subscribe(p =>
                {
                    proximity.Add(DateTime.UtcNow.Ticks, p);
                }));
            }

            if (HeartRate != null)
            {
                availableSensors.Add(HeartRate.WhenReadingTaken().Subscribe(h =>
                {
                    heartRate.Add(DateTime.UtcNow.Millisecond, h);
                }));
            }
        }

        private async void Login()
        {
            if (PassWord == "9Ib_ha@Z@q")
            {
                await App.Current.MainPage.Navigation.PushAsync(new SensorPage());
            }
        }

        private string _uname;
        public string Uname
        {
            get
            {
                return _uname;
            }
            set
            {
                _uname = value;
                OnPropertyChanged("Uname");
            }
        }

        private string _passWord;
        public string PassWord
        {
            get
            {
                return _passWord;
            }
            set
            {
                _passWord = value;
                OnPropertyChanged("PassWord");
            }
        }

        internal void DisposeSubscribers()
        {
            foreach(IDisposable disposable in availableSensors)
            {
                if(disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        public ICommand LoginCommand { get; set; }
        public ICommand FingerPrintCommand { get; set; }
    }
}
