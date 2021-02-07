using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Essentials;

namespace SensorData.ShinySensor.Sensors_XamEssential
{
    public class GyroscopeCapture : ISenseors
    {
        Stopwatch GyroWatch;
        public Dictionary<long, GyroscopeData> GyroscopeDataReading { get; private set; }
        public GyroscopeCapture()
        {
            GyroWatch = new Stopwatch();
            GyroscopeDataReading = new Dictionary<long, GyroscopeData>();
            Gyroscope.ReadingChanged += GyroscopeSensor_ReadingChanged;
        }

        private void GyroscopeSensor_ReadingChanged(object sender, GyroscopeChangedEventArgs e)
        {
            GyroscopeDataReading.Add(GyroWatch.ElapsedTicks, e.Reading);
        }

        public void ControlSunscribe(bool flag)
        {
            try
            {
                if (Gyroscope.IsMonitoring && !flag)
                {
                    Gyroscope.Stop();
                    GyroWatch.Reset();
                }
                else if (!Gyroscope.IsMonitoring && flag)
                {
                    GyroWatch.Start();
                    Gyroscope.Start(Config.sensorSpeed);
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
            GyroscopeDataReading = new Dictionary<long, GyroscopeData>();
        }
    }
}
