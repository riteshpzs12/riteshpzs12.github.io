using System.Windows.Input;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using SensorData.Models;
using SensorData.Services;
using SensorData.ShinySensor.Sensors_XamEssential;
using Xamarin.Forms;

namespace SensorData.ViewModel.FirstPage
{
    //TODO : Only first time when we open the login page if creds are saved then show popup
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
            RegisterCommand = new Command(() => _navService.Goto(new NavigationPage(new SensorData.Views.Registration())));
            if(CheckAndLoadCache())
               FingerPrintLoginAsync();
        }

        /// <summary>
        /// Loads data from cache if available
        /// </summary>
        private bool CheckAndLoadCache()
        {
            App.orientationCapture.ControlSunscribe(true);
            var cred = _cache.Get<CredModel>(Config.CredCacheKey);
            if (cred != null)
            {
                Uname = cred.username;
                PassWord = cred.password;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Tapping on Fingerprint will open the popup for fingerprint
        /// </summary>
        private async void FingerPrintLoginAsync()
        {
            if (await CrossFingerprint.Current.IsAvailableAsync(false))
            {
                _sensorService.StartCapture();
                AuthenticationRequestConfiguration config = new AuthenticationRequestConfiguration("Wait","Let me check who are you");
                if((await CrossFingerprint.Current.AuthenticateAsync(config)).Authenticated)
                {
                    Login();
                }
            }
        }

        /// <summary>
        /// When page reappears reset the sensors
        /// </summary>
        internal void StartOver()
        {
            _sensorService.FlushData();
            CheckAndLoadCache();
        }

        /// <summary>
        /// When page reappears reset the sensors
        /// </summary>
        internal void DisposeSubscribers()
        {
            masterDataModel = _sensorService.DisposeAll();
        }

        /// <summary>
        /// Starts the capturing of the sensor reading
        /// </summary>
        internal void StartCapture()
        {
            _sensorService.StartCapture();
        }

        /// <summary>
        /// Login API call navigation based on response
        /// </summary>
        private async void Login()
        {
            //var l = await _webHelper.TestCompression();
            _sensorService.StartCapture();
            CredModel cred = new CredModel()
            {
                deviceId = App.DeviceId.Trim(),
                username = Uname.Trim(),
                password = PassWord.Trim()
            };
            var res = await _webHelper.PostLoginCall(cred);

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

        /// <summary>
        /// Saves the credentials data based on the response
        /// </summary>
        /// <param name="cred"></param>
        private void SaveDetails(CredModel cred)
        {
            _cache.Add<CredModel>(cred, Config.CredCacheKey);
        }

        /// <summary>
        /// Stops data capturing and shows error if login fails
        /// </summary>
        /// <param name="e"></param>
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
        public ICommand RegisterCommand { get; set; }
    }
}
