using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Essentials;

namespace SensorData.ShinySensor.Sensors_XamEssential
{
    public class AccelerometerCapture : ISenseors
    {
        Stopwatch AccWatch;
        public Dictionary<long, AccelerometerData> AccelerometerDataReading { get; private set; }
        public AccelerometerCapture()
        {
            AccelerometerDataReading = new Dictionary<long, AccelerometerData>();
            AccWatch = new Stopwatch();
            Accelerometer.ReadingChanged += AccelerometerSensor_ReadingChanged;
        }

        private void AccelerometerSensor_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            AccelerometerDataReading.Add(AccWatch.ElapsedTicks, e.Reading);
        }

        public void ControlSunscribe(bool flag)
        {
            try
            {
                if (Accelerometer.IsMonitoring && !flag)
                {
                    Accelerometer.Stop();
                    AccWatch.Reset();
                }
                else if (!Accelerometer.IsMonitoring && flag)
                {
                    AccWatch.Start();
                    Accelerometer.Start(Config.sensorSpeed);
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
