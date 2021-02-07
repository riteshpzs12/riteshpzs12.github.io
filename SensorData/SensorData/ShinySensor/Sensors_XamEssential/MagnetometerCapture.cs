using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Essentials;

namespace SensorData.ShinySensor.Sensors_XamEssential
{
    public class MagnetometerCapture : ISenseors
    {
        Stopwatch MagnetometerWatch;
        public Dictionary<long, MagnetometerData> MagnetometerDataReading { get; private set; }
        public MagnetometerCapture()
        {
            MagnetometerWatch = new Stopwatch();
            MagnetometerDataReading = new Dictionary<long, MagnetometerData>();
            Magnetometer.ReadingChanged += Magnetometer_ReadingChanged;
        }

        private void Magnetometer_ReadingChanged(object sender, MagnetometerChangedEventArgs e)
        {
            MagnetometerDataReading.Add(MagnetometerWatch.ElapsedTicks, e.Reading);
        }

        public void ControlSunscribe(bool flag)
        {
            try
            {
                if (Magnetometer.IsMonitoring && !flag)
                {
                    Magnetometer.Stop();
                    MagnetometerWatch.Reset();
                }
                else if (!Magnetometer.IsMonitoring && flag)
                {
                    MagnetometerWatch.Start();
                    Magnetometer.Start(Config.sensorSpeed);
                }
                else
                {
                    //Dont think anything is needed here
                }
            }
            catch (FeatureNotEnabledException ex)
            {

            }
            catch (Exception ex)
            {

            }
        }

        public void Dispose()
        {
            MagnetometerDataReading = new Dictionary<long, MagnetometerData>();
        }
    }
}
