using System.Windows.Input;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using SensorData.Models;
using SensorData.Services;
using Xamarin.Forms;

namespace SensorData.ViewModel.FirstPage
{
    public class FirstPageViewModel : BaseViewModel
    {
        ISensorService _sensorService;
        MasterDataModel masterDataModel;
        INavService _navService;

        public FirstPageViewModel(ISensorService sensorService, INavService navService)
        {
            _sensorService = sensorService;
            _navService = navService;

            LoginCommand = new Command(Login);
            FingerPrintCommand = new Command(FingerPrintLoginAsync);
        }

        private async void FingerPrintLoginAsync(object obj)
        {
            _sensorService.StartCapture();
            if (await CrossFingerprint.Current.IsAvailableAsync(false))
            {
                AuthenticationRequestConfiguration config = new AuthenticationRequestConfiguration("Wait","Let me check who are u");
                if((await CrossFingerprint.Current.AuthenticateAsync(config)).Authenticated)
                {
                    PassWord = "Sensor@123";
                    Login();
                }
            }
        }

        internal void StartOver()
        {
            _sensorService.FlushData();
            Uname = "";
            PassWord = "";
        }

        internal void DisposeSubscribers()
        {
            masterDataModel = _sensorService.DisposeAll();
        }

        internal void StartCapture()
        {
            _sensorService.StartCapture();
        }
        
        private async void Login()
        {


            if (PassWord == "Sensor@123")
            {
                DisposeSubscribers();
                _navService.Goto(new NavigationPage(new SensorData.Views.PrecisionPredictionTapPage()));
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

        public ICommand LoginCommand { get; set; }
        public ICommand FingerPrintCommand { get; set; }
    }
}
