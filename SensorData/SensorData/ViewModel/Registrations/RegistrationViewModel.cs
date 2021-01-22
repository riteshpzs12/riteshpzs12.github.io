using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SensorData.ViewModel.Registrations
{
    public class RegistrationViewModel : BaseViewModel
    {
        public RegistrationViewModel()
        {
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

        //private bool _privacyFlag = false;
        //public bool PrivacyFlag
        //{
        //    get
        //    {
        //        return _privacyFlag;
        //    }
        //    set
        //    {
        //        _privacyFlag = value;
        //        OnPropertyChanged("PrivacyFlag");
        //    }
        //}
    }
}
