using System;
using System.Windows.Input;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using SensorData.Models;
using SensorData.Services;
using Xamarin.Forms;

namespace SensorData.ViewModel.FirstPage
{
    //TODO : Only first time when we open the login page if creds are saved then show popup
    public class FirstPageViewModel : BaseViewModel
    {
        INavService _navService;
        //public string DeviceId;

        internal void FillDetails(CredModel cred)
        {
            Uname = cred.username;
            PassWord = cred.password;
            if(string.IsNullOrEmpty(App.DeviceId))
                App.DeviceId = cred.deviceId;
            FingerPrintLoginAsync();
            //Show the fingerprint thing.. will see about the user consent and permission thing
        }

        readonly ICache _cache;
        readonly IWebHelper _webHelper;

        public FirstPageViewModel(INavService navService, ICache cache, IWebHelper webHelper)
        {
            _navService = navService;
            _cache = cache;
            _webHelper = webHelper;

            LoginCommand = new Command(Login);
            FingerPrintCommand = new Command(FingerPrintLoginAsync);
            RegisterCommand = new Command(() => _navService.Goto(new NavigationPage(new SensorData.Views.Registration())));
        }

        //Mostly not needed
        /// <summary>
        /// Loads data from cache if available
        /// </summary>
        //private bool CheckAndLoadCache()
        //{
        //    var cred = _cache.Get<CredModel>(Config.CredCacheKey);
        //    if (cred != null)
        //    {
        //        Uname = cred.username;
        //        PassWord = cred.password;
        //        return true;
        //    }
        //    return false;
        //}

        /// <summary>
        /// Tapping on Fingerprint will open the popup for fingerprint
        /// </summary>
        private async void FingerPrintLoginAsync()
        {
            if(!string.IsNullOrEmpty(Uname) && !string.IsNullOrEmpty(PassWord))
            {
                if (await CrossFingerprint.Current.IsAvailableAsync(true))
                {
                    AuthenticationRequestConfiguration config = new AuthenticationRequestConfiguration("Wait", "Let me check who are you");
                    if ((await CrossFingerprint.Current.AuthenticateAsync(config)).Authenticated)
                    {
                        Login();
                    }
                }
            }
        }

        // this wont be needed as once login is successful pressing back wont take user to login screen
        /// <summary>
        /// When page reappears reset the sensors
        /// </summary>
        //internal void StartOver()
        //{
        //    _sensorService.FlushData();
        //    CheckAndLoadCache();
        //}

        //Not needed
        /// <summary>
        /// When page reappears reset the sensors
        /// </summary>
        //internal void DisposeSubscribers()
        //{
        //    masterDataModel = _sensorService.DisposeAll();
        //}

        //not needed
        /// <summary>
        /// Starts the capturing of the sensor reading
        /// </summary>
        //internal void StartCapture()
        //{
        //    _sensorService.StartCapture();
        //}

        /// <summary>
        /// Login API call navigation based on response
        /// </summary>
        private async void Login()
        {
            CredModel cred = new CredModel()
            {
                deviceId = "",
                username = Uname.Trim(),
                password = PassWord.Trim()
            };

            //Commented just for development purpose
            //var res = await _webHelper.PostLoginCall(cred);

            //remove this post development
            var res = new BaseResponse<LoginResponse>.Success()
            {
                data = new LoginResponse()
                {
                     sessionId = "MockSessionId",
                     CreatedOn = DateTime.Now.ToString(),
                     Greet = "Hey Hello",
                     UID = 123
                }
            };
            switch (res)
            {
                case BaseResponse<LoginResponse>.Success s:
                    SaveDetails(cred, s.data);
                    //DisposeSubscribers();
                    _navService.OpenLandingPagePostLogin(new SensorData.Views.LandingPages());
                    break;
                //Commented just for development purpose
                //case BaseResponse<LoginResponse>.Error e:
                //    CheckAndDisplayProperAlert(e);
                //    break;
                 default:
                    break;
            }
        }

        ///TODO Here will save the session Id.
        /// <summary>
        /// Saves the credentials data based on the response
        /// </summary>
        /// <param name="cred"></param>
        private void SaveDetails(CredModel cred, LoginResponse data)
        {
            _cache.Add<CredModel>(cred, Config.CredCacheKey);
            _cache.Add<string>(data.sessionId, Config.SessionDataKey);
        }

        /// <summary>
        /// Stops data capturing and shows error if login fails
        /// </summary>
        /// <param name="e"></param>
        private void CheckAndDisplayProperAlert(BaseResponse<LoginResponse>.Error e)
        {
            //DisposeSubscribers();
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
