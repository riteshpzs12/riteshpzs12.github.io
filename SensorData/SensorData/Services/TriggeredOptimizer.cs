using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using SensorData.Models;
using Xamarin.Essentials;

namespace SensorData.Services
{
    public class TriggeredOptimizer
    {
        private Timer timer { get; set; }
        private ICache _cache { get; set; }
        readonly IWebHelper webHelper;

        public TriggeredOptimizer(ICache cache)
        {
            _cache = cache;
            webHelper = new WebHelper(_cache);
            timer = new Timer(TimeSpan.FromSeconds(30).TotalMilliseconds);
            timer.AutoReset = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(OptimizeAndAdd);
        }

        public void Start()
        {
            if (!timer.Enabled)
            {
                timer.Start();
            }
        }

        public async void Stop()
        {
            if(timer.Enabled)
            {
                OptimizeAndAdd(null, null);
                timer.Stop();
            }
        }

        private async void OptimizeAndAdd(object sender, ElapsedEventArgs e)
        {
            try
            {
                var data = App.sensorService.GetData();
                App.sensorService.FlushData();
                var d = _cache.Get<MasterDataModel>(Config.CacheDataKey);
                List<bool> done = new List<bool>();
                if (d != null)
                {
                    done.Add(await webHelper.SendSensorData<AccelerometerData>(data.AccelerometerData, SensorTypeEnum.Accelerometer));
                    done.Add(await webHelper.SendSensorData<MagnetometerData>(data.MagnetometerData, SensorTypeEnum.Magnetometer));
                    done.Add(await webHelper.SendSensorData<OrientationSensorData>(data.OrientationSensorData, SensorTypeEnum.Orientation));
                    done.Add(await webHelper.SendSensorData<GyroscopeData>(data.GyroscopeData, SensorTypeEnum.Gyroscope));
                    done.Add(await webHelper.SendSensorData<CompassData>(data.CompassData, SensorTypeEnum.Compass));
                    await App.Current.MainPage.DisplayAlert("Results", string.Join(", ", done), "Ok");
                }
                _cache.Add<MasterDataModel>(data, Config.CacheDataKey);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
