using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using SensorData.Models;
using SensorData.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SensorData.ViewModel.Registrations
{
    public class RegistrationViewModel : BaseViewModel
    {
        readonly INavService _navservice;
        readonly ICache _cache;
        readonly IWebHelper _webHelper;
        readonly IUtility _utility;

        public RegistrationViewModel(INavService navservice, ICache cache, IWebHelper webHelper)
        {
            _cache = cache;
            _navservice = navservice;
            _webHelper = webHelper;
            _utility = DependencyService.Get<IUtility>();

            RegisterUser = new Command(Register);
            ConsentGiven = new Command(() => IsConsentVisible = false);
            ListItems.Add(new Consents()
            {
                Id = 1,
                Description = "Your Device Sensor Data",
                Allowed = true
            });
            ListItems.Add(new Consents()
            {
                Id = 2,
                Description = "Your Device Model Name",
                Allowed = true
            });
        }

        private async void Register()
        {
            //TODO Do the API call take the response save the installation token
            if(await Permissions.CheckStatusAsync<Permissions.Phone>() == PermissionStatus.Granted)
                _utility.GetAppId();
            else
            {
                //will think the else one
            }
            RegisterModel register = new RegisterModel()
            {
                Name = Name,
                Contact = EmailorPhone,
                DeviceId = App.DeviceId,
                DeviceModel = Xamarin.Essentials.DeviceInfo.Manufacturer + ", " + Xamarin.Essentials.DeviceInfo.Model,
                Password = PassWord,
                Consent = GetConsent(),
                DeviceType = Xamarin.Essentials.DeviceInfo.Platform + "  " + Xamarin.Essentials.DeviceInfo.Idiom
            };

            //Commented just for development purpose
            //var response = await _webHelper.PostRegister(register);

            //remove this post development
            var response = new BaseResponse<RegistrationResponse>.Success()
            {
                data = new RegistrationResponse()
                {
                    InstallToken = "MockInstallToken",
                    UID = 23323
                }
            };
            switch (response)
            {
                case BaseResponse<RegistrationResponse>.Success s:
                    SaveToken(s.data.InstallToken);
                    _navservice.Goto(new SensorData.Views.FirstPage(new CredModel
                    {
                        deviceId = App.DeviceId,
                        password = PassWord,
                        username = Name
                    }));
                    break;
                //Commented just for development purpose
                //case BaseResponse<RegistrationResponse>.Error e:
                //    var res = await _navservice.ShowInteractiveDialogAsync("Sorry", "Sorry we are experiencing some trouble. Please try again " +
                //        "after sometime or Contact us", "Call Us", "Cancel");
                //    if (res)
                //    {
                //        //Open the phone dialer 
                //    }
                //    break;
                default:
                    break;
            }
        }

        private void SaveToken(string installToken)
        {
            Xamarin.Essentials.SecureStorage.SetAsync(Config.InstallationToken, installToken);
        }

        private string GetConsent()
        {
            string res = "";
            foreach (Consents consents in ListItems)
            {
                res += consents.Allowed ? "1" : "0";
            };
            return res;
        }

        internal async void ResolvePermissions()
        {
            try
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    var Sensorstatus = await Permissions.CheckStatusAsync<Permissions.Sensors>();
                    if (Sensorstatus != PermissionStatus.Granted)
                    {

                        if (await Permissions.RequestAsync<Permissions.Sensors>() != PermissionStatus.Granted)
                        {
                            //we can have a custom alert message here
                            await App.Current.MainPage.DisplayAlert("Need Permission", "Need Sensor Permission", "Ok");
                            await Permissions.RequestAsync<Permissions.Sensors>();
                        }
                    }

                    var Phonestatus = await Permissions.CheckStatusAsync<Permissions.Phone>();
                    if (Sensorstatus != PermissionStatus.Granted)
                    {
                        if (await Permissions.RequestAsync<Permissions.Phone>() != PermissionStatus.Granted)
                        {
                            //we can have a custom alert message here
                            await App.Current.MainPage.DisplayAlert("Need Permission", "Need Phone Status Permission", "Ok");
                            await Permissions.RequestAsync<Permissions.Phone>();
                        }
                    }

                    var Storagestatus = await Permissions.RequestAsync<Permissions.StorageWrite>();
                    if (Storagestatus != PermissionStatus.Granted)
                    {
                        if (await Permissions.RequestAsync<Permissions.StorageWrite>() != PermissionStatus.Granted)
                        {
                            //we can have a custom alert message here
                            await App.Current.MainPage.DisplayAlert("Need Permission", "Need Storage Write Permission", "Ok");
                            await Permissions.RequestAsync<Permissions.StorageWrite>();
                        }
                    }

                    var Storagereadstatus = await Permissions.RequestAsync<Permissions.StorageRead>();
                    if (Storagereadstatus != PermissionStatus.Granted)
                    {
                        if (await Permissions.RequestAsync<Permissions.StorageRead>() != PermissionStatus.Granted)
                        {
                            //we can have a custom alert message here
                            await App.Current.MainPage.DisplayAlert("Need Permission", "Need Storage Read Permission", "Ok");
                            await Permissions.RequestAsync<Permissions.StorageRead>();
                        }
                    }

                    //Will handle this scenarioss..
                    //if (status == PermissionStatus.Granted)
                    //{
                    //    //Query permission
                    //}
                    //else if (status != PermissionStatus.Unknown)
                    //{
                    //    //location denied
                    //}
                }
            }
            catch (Exception ex)
            {
                //Something went wrong
            }
        }

        private string _name = "";
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private string _passWord = "";
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

        private bool _isConsentVisible = false;
        public bool IsConsentVisible
        {
            get
            {
                return _isConsentVisible;
            }
            set
            {
                _isConsentVisible = value;
                OnPropertyChanged("IsConsentVisible");
            }
        }

        internal void ShowConsent()
        {
            IsConsentVisible = true;
        }

        private ObservableCollection<Consents> _listItems = new ObservableCollection<Consents>();
        public ObservableCollection<Consents> ListItems
        {
            get
            {
                return _listItems;
            }
            set
            {
                _listItems = value;
                OnPropertyChanged("ListItems");
            }
        }

        private string _emailorPhone = "";
        public string EmailorPhone
        {
            get
            {
                return _emailorPhone;
            }
            set
            {
                _emailorPhone = value;
                OnPropertyChanged("EmailorPhone");
            }
        }

        private string _confirmPassword = "";
        public string ConfirmPassword
        {
            get
            {
                return _confirmPassword;
            }
            set
            {
                _confirmPassword = value;
                OnPropertyChanged("ConfirmPassword");
            }
        }

        private bool _isRegisterEnabled = false;
        public bool IsRegisterEnabled
        {
            get
            {
                return _isRegisterEnabled;
            }
            set
            {
                _isRegisterEnabled = value;
                OnPropertyChanged("IsRegisterEnabled");
            }
        }

        internal bool[] validate()
        {
            var res = new bool[4];
            if (!string.IsNullOrEmpty(Name))
                res[0] = true;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(EmailorPhone);
            if (match.Success)
                res[1] = true;
            else
            {
                var phone = new string(EmailorPhone.Where(Char.IsDigit).ToArray());
                if (phone.Length == 10)
                    res[1] = true;
            }
            if (PassWord.Length>5)
            {
                if (PassWord.Where(Char.IsDigit).Count() > 0 && PassWord.Where(Char.IsLetter).Count() > 0)
                    res[2] = true;
            }
            if(ConfirmPassword.Length > 5)
            {
                if (ConfirmPassword == PassWord && res[2])
                    res[3] = true;
            }
            return res;
        }

        public ICommand RegisterUser { get; private set; }
        public ICommand ConsentGiven { get; private set; }
    }

    public class Consents
    {
        public int Id { get; set; }
        public bool Allowed { get; set; }
        public string Description { get; set; }
    }
}
