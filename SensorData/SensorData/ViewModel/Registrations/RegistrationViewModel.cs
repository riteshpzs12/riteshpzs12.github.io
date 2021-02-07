using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using SensorData.Services;
using Xamarin.Forms;

namespace SensorData.ViewModel.Registrations
{
    public class RegistrationViewModel : BaseViewModel
    {
        readonly INavService _navservice;
        //readonly IFileOperation _fileOperation;
        readonly ICache _cache;
        List<Permission> permissions = new List<Permission>()
        {
            Permission.Storage,
            Permission.Sensors
        };

        public RegistrationViewModel(INavService navservice, ICache cache)
        {
            _cache = cache;
            //_fileOperation = fileOperation;
            _navservice = navservice;
            StartConsent = new Command(ResolveConsent);
            RegisterUser = new Command(Register);
        }

        private void Register()
        {
            //TODO Do the API call take the response save the installation token
            _navservice.Goto(new NavigationPage(new SensorData.Views.FirstPage()));
        }

        internal async void ResolvePermissions()
        {
            try
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    var Sensorstatus = await CrossPermissions.Current.CheckPermissionStatusAsync<SensorsPermission>();
                    if (Sensorstatus != PermissionStatus.Granted)
                    {
                        if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Sensors))
                        {
                            //we can have a custom alert message here
                            await App.Current.MainPage.DisplayAlert("Need Permission", "Need Sensor Permission", "Ok");
                        }

                        Sensorstatus = await CrossPermissions.Current.RequestPermissionAsync<SensorsPermission>();
                    }

                    var Storagestatus = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
                    if (Storagestatus != PermissionStatus.Granted)
                    {
                        if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                        {
                            //we can have a custom alert message here
                            await App.Current.MainPage.DisplayAlert("Need Permission", "Need Storage Permission", "Ok");
                        }

                        Storagestatus = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
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

        private void ResolveConsent()
        {
            ///Show the consents popup
            ///With fields options like
            ///1. Sensor data
            ///2. Device Model Name
            ///3. TODO
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

        public ICommand StartConsent { get; private set; }
        public ICommand RegisterUser { get; private set; }
    }
}
