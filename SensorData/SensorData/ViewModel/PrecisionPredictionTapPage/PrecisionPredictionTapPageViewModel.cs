using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SensorData.Services;
using SensorData.Views.CustomViews;
using Xamarin.Forms;

namespace SensorData.ViewModel.PrecisionPredictionTapPage
{
    public class PrecisionPredictionTapPageViewModel : BaseViewModel
    {
        INavService _navService;
        public double XTop = -1;
        public double YTop = -1;
        public double XPlay = -1;
        public double YPlay = -1;
        public List<TapPrecisionModel> UserTapPrecisionData;
        public PrecisionPredictionTapPageViewModel(INavService navService)
        {
            _navService = navService;
            UserTapPrecisionData = new List<TapPrecisionModel>();
            _statDisplayModels = new ObservableCollection<StatDisplayModel>();
        }

        private ObservableCollection<StatDisplayModel> _statDisplayModels;
        public ObservableCollection<StatDisplayModel> StatDisplayModels
        {
            get
            {
                return _statDisplayModels;
            }
            set
            {
                _statDisplayModels = value;
                OnPropertyChanged("StatDisplayModels");
            }
        }

        private double _XLocationFactor;
        public double XLocationFactor
        {
            get
            {
                return _XLocationFactor;
            }
            set
            {
                _XLocationFactor = value;
                OnPropertyChanged("XLocationFactor");
            }
        }

        private double _ActualX;
        public double ActualX
        {
            get
            {
                return _ActualX;
            }
            set
            {
                _ActualX = value;
                OnPropertyChanged("ActualX");
            }
        }

        private double _ActualY;
        public double ActualY
        {
            get
            {
                return _ActualY;
            }
            set
            {
                _ActualY = value;
                OnPropertyChanged("ActualY");
            }
        }

        private double _UserX;
        public double UserX
        {
            get
            {
                return _UserX;
            }
            set
            {
                _UserX = value;
                OnPropertyChanged("UserX");
            }
        }

        private double _UserY;
        public double UserY
        {
            get
            {
                return _UserY;
            }
            set
            {
                _UserY = value;
                OnPropertyChanged("UserY");
            }
        }

        private bool _IsStatVisible = false;
        public bool IsStatVisible
        {
            get
            {
                return _IsStatVisible;
            }
            set
            {
                _IsStatVisible = value;
                OnPropertyChanged("IsStatVisible");
            }
        }

        private bool _PositionVisiblity = false;
        public bool PositionVisiblity
        {
            get
            {
                return _PositionVisiblity;
            }
            set
            {
                _PositionVisiblity = value;
                OnPropertyChanged("PositionVisiblity");
            }
        }

        public void UpdateDotLocation()
        {
            Random random = new Random();
            if(XTop!=-1 && YTop!=-1)
            {
                XLocationFactor = (random.NextDouble() * (.9) + .05)*XTop;
                YLocationFactor = (random.NextDouble() * (.9) + .05)*YTop;
            }
        }

        private double _YLocationFactor;
        public double YLocationFactor
        {
            get
            {
                return _YLocationFactor;
            }
            set
            {
                _YLocationFactor = value;
                OnPropertyChanged("YLocationFactor");
            }
        }

        public Command<TestTry> ResetCommand { get { return new Command<TestTry>((t) => Reset(t)); } }

        private void Reset(TestTry testTry)
        {
            if (testTry.CustomField1 == "Up")
            {
                IsStatVisible = false;
                StatDisplayModels.Clear();
                UpdateDotLocation();
                PositionVisiblity = false;
            } 
        }

        public Command<Point> PrecisionCommand { get { return new Command<Point>((p) => CheckPrecisionAsync(p)); } }

        private async System.Threading.Tasks.Task CheckPrecisionAsync(Point point)
        {
            if (!IsStatVisible)
            {
                TapPrecisionModel acc = CalculateAccuracy(point);
                UpdatePlayGround(point, acc);
                var totalAcc = (100 - (acc.UserXError + acc.UserYError));
                var choice = await _navService.ShowInteractiveDialogAsync("Accuracy", "Your accuracy is :  " + totalAcc.ToString("#.000"), "Play More", "Stats");
                if (choice)
                {
                    UpdateDotLocation();
                    PositionVisiblity = false;
                }
                else
                {
                    showStat();
                }
            }
        }

        private void showStat()
        {
            IsStatVisible = true;
            StatDisplayModels.Add(new StatDisplayModel() {
                Key = "Last Accuracy",
                Value = (100 - (UserTapPrecisionData[UserTapPrecisionData.Count-1].UserXError + UserTapPrecisionData[UserTapPrecisionData.Count - 1].UserYError)).ToString()
            });
            StatDisplayModels.Add(new StatDisplayModel()
            {
                Key = "Last Horizontal Error",
                Value = UserTapPrecisionData[UserTapPrecisionData.Count - 1].UserXError.ToString()
            });
            StatDisplayModels.Add(new StatDisplayModel()
            {
                Key = "Last Vertical Error",
                Value = UserTapPrecisionData[UserTapPrecisionData.Count - 1].UserYError.ToString()
            });
            double totalYError = 0.0;
            double totalXError = 0.0;
            UserTapPrecisionData.ForEach(u => { totalXError += u.UserXError; totalYError += u.UserYError; });
            StatDisplayModels.Add(new StatDisplayModel()
            {
                Key = "Average Accuracy",
                Value = ((UserTapPrecisionData.Count*100 - totalYError - totalXError)/UserTapPrecisionData.Count).ToString()
            });
            StatDisplayModels.Add(new StatDisplayModel()
            {
                Key = "Average Horizontal Error",
                Value = (totalXError/UserTapPrecisionData.Count).ToString()
            });
            StatDisplayModels.Add(new StatDisplayModel()
            {
                Key = "Average Vertical Error",
                Value = (totalYError/UserTapPrecisionData.Count).ToString()
            });
        }

        private void UpdatePlayGround(Point point, TapPrecisionModel acc)
        {
            PositionVisiblity = true;
            UserTapPrecisionData.Add(acc);
            ActualX = acc.ActualXPosition * XPlay;
            ActualY = acc.ActualYPosition * YPlay;

            UserX = point.X;
            UserY = point.Y;
        }

        private TapPrecisionModel CalculateAccuracy(Point point)
        {
            TapPrecisionModel res = new TapPrecisionModel();
            if(XTop != -1 && XPlay != -1 && YTop != -1 && YPlay != -1)
            {
                res.ActualXPosition = XLocationFactor / XTop;
                res.ActualYPosition = YLocationFactor / YTop;

                res.UserXError = Math.Abs((point.X / XPlay) - res.ActualXPosition)*100;
                res.UserYError = Math.Abs((point.Y / YPlay) - res.ActualYPosition)*100;
                /////Some logic needed to find out the percentage accuracy from this variance
            }
            return res;
        }
    }

    public class TapPrecisionModel
    {
        public double ActualXPosition { get; set; }
        public double ActualYPosition { get; set; }
        public double UserXError { get; set; }
        public double UserYError { get; set; }
    }

    public class StatDisplayModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
