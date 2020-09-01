using System;
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
        ICache _cache;
        IWebHelper _webHelper;

        public FirstPageViewModel(ISensorService sensorService, INavService navService, ICache cache, IWebHelper webHelper)
        {
            _sensorService = sensorService;
            _navService = navService;
            _cache = cache;
            _webHelper = webHelper;

            LoginCommand = new Command(Login);
            FingerPrintCommand = new Command(FingerPrintLoginAsync);
            CheckAndLoadCache();
        }

        private void CheckAndLoadCache()
        {
            var cred = _cache.Get<CredModel>(Config.CredCacheKey);
            if (cred != null)
            {
                Uname = cred.userName;
                PassWord = cred.passWord;
                FingerPrintLoginAsync();
            }
        }

        private async void FingerPrintLoginAsync()
        {
            if (await CrossFingerprint.Current.IsAvailableAsync(false))
            {
                _sensorService.StartCapture();
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
            _sensorService.StartCapture();
            CredModel cred = new CredModel()
            {
                deviceId = App.DeviceId.Trim(),
                userName = Uname.Trim(),
                passWord = PassWord.Trim()
            };
            var res = await _webHelper.PostCall(cred);

            switch (res)
            {
                case BaseResponse<LoginResponse>.Success s:
                    SaveDetails(cred);
                    DisposeSubscribers();
                    _navService.Goto(new NavigationPage(new SensorData.Views.PrecisionPredictionTapPage()));
                    break;
                case BaseResponse<LoginResponse>.Error e:
                    CheckAndDisplayProperAlert(e);
                    break;
                 default:
                    break;
            }
        }

        private void SaveDetails(CredModel cred)
        {
            _cache.Add<CredModel>(cred, Config.CredCacheKey);
        }

        private void CheckAndDisplayProperAlert(BaseResponse<LoginResponse>.Error e)
        {
            DisposeSubscribers();
            _navService.ShowDialog("ERROR", e.message);
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
