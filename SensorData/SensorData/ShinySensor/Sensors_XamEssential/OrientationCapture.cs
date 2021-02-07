using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Essentials;

namespace SensorData.ShinySensor.Sensors_XamEssential
{
    public class OrientationCapture : ISenseors
    {
        Stopwatch OrientationWatch;
        public Dictionary<long, OrientationSensorData> OrientationDataReading { get; private set; }
        public OrientationCapture()
        {
            OrientationWatch = new Stopwatch();
            OrientationDataReading = new Dictionary<long, OrientationSensorData>();
            OrientationSensor.ReadingChanged += OrientationSensor_ReadingChanged;
        }

        private void OrientationSensor_ReadingChanged(object sender, OrientationSensorChangedEventArgs e)
        {
            OrientationDataReading.Add(OrientationWatch.ElapsedTicks, e.Reading);
        }

        public void ControlSunscribe(bool flag)
        {
            try
            {
                if (OrientationSensor.IsMonitoring && !flag)
                {
                    OrientationSensor.Stop();
                    OrientationWatch.Reset();
                }
                else if (!OrientationSensor.IsMonitoring && flag)
                {
                    OrientationWatch.Start();
                    OrientationSensor.Start(Config.sensorSpeed);
                }
                else
                {
                    //Dont think anything is needed here
                }
            }
            catch(FeatureNotEnabledException ex)
            {

            }
            catch(Exception ex)
            {

            }
        }

        public void Dispose()
        {
            OrientationDataReading = new Dictionary<long, OrientationSensorData>();
        }
    }
}
