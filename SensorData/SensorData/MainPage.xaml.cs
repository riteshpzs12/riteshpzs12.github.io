using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SensorData.ShinySensor;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SensorData
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        SensorSpeed speed = SensorSpeed.UI;

        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = this;
            OrientationSensor.ReadingChanged += OrientationSensor_ReadingChanged;
            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var reading = e.Reading;
            //if(Math.Abs(AccX - reading.Acceleration.X)>.001)
                AccX = reading.Acceleration.X;
            //if (Math.Abs(AccY - reading.Acceleration.Y) > .001)
                AccY = reading.Acceleration.Y;
            //if (Math.Abs(AccZ - reading.Acceleration.Z) > .001)
                AccZ = reading.Acceleration.Z;
        }

        private void OrientationSensor_ReadingChanged(object sender, OrientationSensorChangedEventArgs e)
        {
            var reading = e.Reading;
            //if (Math.Abs(AccX - reading.Acceleration.X) > .001)
                OccX = reading.Orientation.X;
            //if (Math.Abs(AccY - reading.Acceleration.Y) > .001)
                OccY = reading.Orientation.Y;
            //if (Math.Abs(AccZ - reading.Acceleration.Z) > .001)
                OccZ = reading.Orientation.Z;
        }

        public void ToggleOrientationSensor()
        {
            try
            {
                if (OrientationSensor.IsMonitoring)
                    OrientationSensor.Stop();
                else
                    OrientationSensor.Start(speed);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
            }
        }

        public void ToggleAccelerometer()
        {
            try
            {
                if (Accelerometer.IsMonitoring)
                    Accelerometer.Stop();
                else
                    Accelerometer.Start(speed);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
            }
        }

        private float occX = 0.0F;
        public float OccX
        {
            get
            {
                return occX;
            }
            set
            {
                occX = value;
                NotifyPropertyChanged("OccX");
            }
        }

        private float occY = 0.0F;
        public float OccY
        {
            get
            {
                return occY;
            }
            set
            {
                occY = value;
                NotifyPropertyChanged("OccY");
            }
        }

        private float occZ = 0.0F;
        public float OccZ
        {
            get
            {
                return occZ;
            }
            set
            {
                occZ = value;
                NotifyPropertyChanged("OccZ");
            }
        }

        private float accX = 0.0F;
        public float AccX
        {
            get
            {
                return accX;
            }
            set
            {
                accX = value;
                NotifyPropertyChanged("AccX");
            }
        }

        private float accY = 0.0F;
        public float AccY
        {
            get
            {
                return accY;
            }
            set
            {
                accY = value;
                NotifyPropertyChanged("AccY");
            }
        }

        private float accZ = 0.0F;
        public float AccZ
        {
            get
            {
                return accZ;
            }
            set
            {
                accZ = value;
                NotifyPropertyChanged("AccZ");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            ToggleOrientationSensor();
            ToggleAccelerometer();
        }

        void Button_Clicked_1(System.Object sender, System.EventArgs e)
        {
            var enumerator = SensorStartup.DeviceSensorsAvailable.GetEnumerator();
            string data = "";
            while(enumerator.MoveNext())
            {
                data = data + enumerator.Current.Key + enumerator.Current.Value.ToString() + "/////////////////";
            }
            App.Current.MainPage.DisplayAlert("SENSOR DATALIST", data, "OK");
        }
    }
}
