using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using SensorData.Models;
using Xamarin.Forms;

namespace SensorData.Views
{
    public partial class SensorPage : ContentPage
    {
        public SensorPage(MasterDataModel masterDataModel = null)
        {
            InitializeComponent();
            if (masterDataModel == null)
                App.Current.MainPage.DisplayAlert("OH NOOOOO", "Failed To Capture Sensor Data. Please Try Again", "ok");
            else
            {
                Compass.Text = "Compass Readings Count : " + masterDataModel.CompassData.Count;
                Gyro.Text = "Gyroscope Readings Count : " + masterDataModel.GyroscopeData.Count;
                Accel.Text = "Accelerometer Readings Count : " + masterDataModel.AccelerometerData.Count;
                Proximity.Text = "Proximity Readings Count : " + masterDataModel.ProximityData.Count;
                Heart.Text = "HeartRate Readings Count : " + masterDataModel.HeartRateData.Count;
            }
        }

        private async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                HttpClient client = new HttpClient();

                var uri = new Uri("http://ec2-3-94-134-168.compute-1.amazonaws.com:8080/unlock-sensor");
                TestBody testBody = new TestBody()
                {
                    gyroscope = Gyro.Text,
                    accelerometer = Accel.Text
                };
                var buffer = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(testBody));
                //var buffer = System.Text.Encoding.UTF8.GetBytes(Compass.Text+"///"+Gyro.Text+"///"+Accel.Text+"///"+Proximity.Text+"////");
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue(@"application/json");

                var response = await client.PostAsync(uri, byteContent);
                if(response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    await App.Current.MainPage.DisplayAlert("Success", result.ToString(), "ok");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Meghaaaaa", "YOUR CODE SUCKSSSSSSSSSSSS", "ok");
            }
        }
    }

    class TestBody
    {       
        public string gyroscope;
        public string accelerometer;
    }
}
