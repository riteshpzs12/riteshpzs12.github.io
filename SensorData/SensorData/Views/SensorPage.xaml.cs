using System;
using System.Collections.Generic;
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
    }
}
