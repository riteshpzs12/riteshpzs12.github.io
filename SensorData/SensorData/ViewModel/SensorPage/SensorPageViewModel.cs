using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using SensorData.Models;
using SensorData.Services;
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

        public Command<Point> SendDataCommand { get { return new Command<Point>((p) => SendAllData(p)); } }
        public Command<Point> ParentLayoutCommand { get { return new Command<Point>((p) => OnTapped(p)); } }
        public Command<Point> EntryCommand { get { return new Command<Point>((p) => OnTapped(p)); } }
        public Command<Point> ImageCommand { get { return new Command<Point>((p) => OnTapped(p)); } }
        public Command<Point> LabelCommand { get { return new Command<Point>((p) => OnTapped(p)); } }
        public Command<Point> TappedCommand { get { return new Command<Point>((p) => OnTapped(p)); } }
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
                OnPropertyChanged("SensorData");
            }
        }

        private void SendAllData(object obj)
        {
            App.Current.MainPage.DisplayAlert("Cooooool", "X coordinate : " + ((Point)obj).X + "        Y coordinate : " + ((Point)obj).Y, "OKKKKK");
            navService.ShowDialog("SemiCool", "Send Data to Server");

            //try
            //{
            //    HttpClient client = new HttpClient();

            //    var uri = new Uri("http://ec2-3-94-134-168.compute-1.amazonaws.com:8080/unlock-sensor");
            //    TestBody testBody = new TestBody()
            //    {
            //        gyroscope = Gyro.Text,
            //        accelerometer = Accel.Text
            //    };
            //    var buffer = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(testBody));
            //    //var buffer = System.Text.Encoding.UTF8.GetBytes(Compass.Text+"///"+Gyro.Text+"///"+Accel.Text+"///"+Proximity.Text+"////");
            //    var byteContent = new ByteArrayContent(buffer);
            //    byteContent.Headers.ContentType = new MediaTypeHeaderValue(@"application/json");

            //    var response = await client.PostAsync(uri, byteContent);
            //    if (response.IsSuccessStatusCode)
            //    {
            //        var result = response.Content.ReadAsStringAsync().Result;
            //        await App.Current.MainPage.DisplayAlert("Success", result.ToString(), "ok");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    await App.Current.MainPage.DisplayAlert("Meghaaaaa", "YOUR CODE SUCKSSSSSSSSSSSS", "ok");
            //}
        }
    }

    class TestBody
    {
        public string gyroscope;
        public string accelerometer;
    }
}
