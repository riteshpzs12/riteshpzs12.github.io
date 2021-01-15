using SensorData.Models;
using SensorData.Services;
using SensorData.Views.CustomViews;
using Xamarin.Forms;

namespace SensorData.ViewModel.SensorPage
{
    public class SensorPageViewModel : BaseViewModel
    {
        INavService navService;

        public SensorPageViewModel(INavService inavService)
        {
            navService = inavService;
        }

        private string compassText = "";
        public string CompassText
        {
            get
            {
                return "Compass Readings Count : " + compassText;
            }
            set
            {
                compassText = value;
                OnPropertyChanged("CompassText");
            }
        }

        private string gyroText = "";
        public string GyroText
        {
            get
            {
                return "Gyro Readings Count : " + gyroText;
            }
            set
            {
                gyroText = value;
                OnPropertyChanged("GyroText");
            }
        }

        private string accelerometerText = "";
        public string AccelerometerText
        {
            get
            {
                return "Accelerometer Readings Count : " + accelerometerText;
            }
            set
            {
                accelerometerText = value;
                OnPropertyChanged("AccelerometerText");
            }
        }

        private string tapDataText = "";
        public string TapDataText
        {
            get
            {
                return tapDataText;
            }
            set
            {
                tapDataText = value;
                OnPropertyChanged("TapDataText");
            }
        }

        private string panDataText = "";
        public string PanDataText
        {
            get
            {
                return panDataText;
            }
            set
            {
                panDataText = value;
                OnPropertyChanged("PanDataText");
            }
        }


        public Command<TestTry> FrameSwipeCommand{ get { return new Command<TestTry>((p) => CheckSwipeData(p)); } }
        public Command<Point> ParentLayoutCommand { get { return new Command<Point>((p) => OnTapped(p)); } }
        
        public void OnTapped(Point p)
        {
            App.Current.MainPage.DisplayAlert("Cooooool", "X coordinate : " + p.X + "        Y coordinate : " + p.Y, "OKKKKK");
        }

        private MasterDataModel sensorData = null;
        public MasterDataModel SensorData
        {
            get
            {
                return sensorData;
            }
            set
            {
                sensorData = value;
                AddorUpdateFields(sensorData);
                OnPropertyChanged("SensorData");
            }
        }

        private void AddorUpdateFields(MasterDataModel sensorData)
        {
            CompassText = sensorData.CompassData.Count.ToString();
            GyroText = sensorData.GyroscopeData.Count.ToString();
            AccelerometerText = sensorData.AccelerometerData.Count.ToString();
        }

        private void CheckSwipeData(TestTry testTry)
        {
            var stringData = "";
            foreach(Point p in testTry.Coordinate)
            {
                stringData = stringData + "X:  " + p.X + "   Y:  " + p.Y + "       ";
            }
            if (!string.IsNullOrEmpty(testTry.CustomField1))
                stringData = stringData + testTry.CustomField1 + "     ";
            if (!string.IsNullOrEmpty(testTry.CustomField2))
                stringData = stringData + testTry.CustomField2;

            //if (Device.RuntimePlatform == Device.Android)
            //    App.Current.MainPage.DisplayAlert("Cooooool", "Not enough data captured Yet      XLEFT...." + testTry.Coordinate[0].X + "     YLEFT......." + testTry.Coordinate[0].Y
            //    + "XRIGHT...." + testTry.Coordinate[1].X + "     YRIGHT......." + testTry.Coordinate[1].Y + "        "+testTry.CustomField1+"         "+testTry.CustomField2, "OKKKKKK");
            ////"XUP...." + testTry.Coordinate[2].X + "     YUP......." + testTry.Coordinate[2].Y
            ////    + "XDOWN...." + testTry.Coordinate[3].X + "     YDOWN......." + testTry.Coordinate[3].Y, "OKKKKK");
            //else
            App.Current.MainPage.DisplayAlert("Cooooool", "THIS IS WHAT IS DONE>>>>" + stringData, "OKKKKKK");
        }
    }

    class TestBody
    {
        public string gyroscope;
        public string accelerometer;
    }
}
