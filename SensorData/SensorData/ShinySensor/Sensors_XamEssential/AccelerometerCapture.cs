using System;
using System.Collections.Generic;
using Xamarin.Essentials;

namespace SensorData.ShinySensor.Sensors_XamEssential
{
    public class AccelerometerCapture : ISenseors
    {
        public Dictionary<long, AccelerometerData> AccelerometerDataReading { get; private set; }
        public AccelerometerCapture()
        {
            Accelerometer.ReadingChanged += AccelerometerSensor_ReadingChanged;
            AccelerometerDataReading = new Dictionary<long, AccelerometerData>();
        }

        private void AccelerometerSensor_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            AccelerometerDataReading.Add(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), e.Reading);
        }

        public void ControlSunscribe(bool flag)
        {
            try
            {
                if (Accelerometer.IsMonitoring && !flag)
                {
                    Accelerometer.Stop();
                }
                else if (!Accelerometer.IsMonitoring && flag)
                {
                    Accelerometer.Start(SensorSpeed.UI);
                }
                else
                {
                    //Dont think anything is needed here
                }
            }
            catch (FeatureNotEnabledException ex)
            {
                //TODO
            }
            catch(Exception ex)
            {
                //TODO
            }
        }

        public void Dispose()
        {
            AccelerometerDataReading = new Dictionary<long, AccelerometerData>();
        }
    }
}
