using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Essentials;

namespace SensorData.ShinySensor.Sensors_XamEssential
{
    public class CompassCapture : ISenseors
    {
        Stopwatch CompassWatch;
        public Dictionary<long, CompassData> CompassDataReading { get; private set; }
        public CompassCapture()
        {
            CompassDataReading = new Dictionary<long, CompassData>();
            CompassWatch = new Stopwatch();
            Compass.ReadingChanged += CompassSensor_ReadingChanged;
        }

        private void CompassSensor_ReadingChanged(object sender, CompassChangedEventArgs e)
        {
            CompassDataReading.Add(CompassWatch.ElapsedTicks, e.Reading);
        }

        public void ControlSunscribe(bool flag)
        {
            try
            {
                if (Compass.IsMonitoring && !flag)
                {
                    Compass.Stop();
                    CompassWatch.Reset();
                }
                else if (!Compass.IsMonitoring && flag)
                {
                    CompassWatch.Start();
                    Compass.Start(Config.sensorSpeed);
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
            CompassDataReading = new Dictionary<long, CompassData>();
        }
    }
}
