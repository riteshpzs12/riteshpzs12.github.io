using System;
using System.Collections.Generic;
using Xamarin.Essentials;

namespace SensorData.ShinySensor.Sensors_XamEssential
{
    public class OrientationCapture : ISenseors
    {
        public Dictionary<long, OrientationSensorData> OrientationDataReading { get; private set; }
        public OrientationCapture()
        {
            OrientationSensor.ReadingChanged += OrientationSensor_ReadingChanged;
        }

        private void OrientationSensor_ReadingChanged(object sender, OrientationSensorChangedEventArgs e)
        {
            OrientationDataReading.Add(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), e.Reading);
            OrientationDataReading = new Dictionary<long, OrientationSensorData>();
        }

        public void ControlSunscribe(bool flag)
        {
            try
            {
                if (OrientationSensor.IsMonitoring && !flag)
                {
                    OrientationSensor.Stop();
                }
                else if (!OrientationSensor.IsMonitoring && flag)
                {
                    OrientationSensor.Start(SensorSpeed.UI);
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
