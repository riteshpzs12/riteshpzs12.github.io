using System;
using System.IO;
using System.Text;
using System.Timers;
using Newtonsoft.Json;
using SensorData.Models;
using Xamarin.Essentials;

namespace SensorData.Services
{
    public class TriggeredOptimizer
    {
        private Timer timer { get; set; }
        private ICache _cache { get; set; }
        IWebHelper webHelper;
        private IFileOperation fileOperation;

        public TriggeredOptimizer(ICache cache)
        {
            _cache = cache;
            fileOperation = new FileOperation();
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

        public void Stop()
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
                if (d != null)
                {
                    await webHelper.SendSensorData<AccelerometerData>(data.AccelerometerData, SensorTypeEnum.Accelerometer);
                    await webHelper.SendSensorData<MagnetometerData>(data.MagnetometerData, SensorTypeEnum.Magnetometer);
                    await webHelper.SendSensorData<OrientationSensorData>(data.OrientationSensorData, SensorTypeEnum.Orientation);
                    await webHelper.SendSensorData<GyroscopeData>(data.GyroscopeData, SensorTypeEnum.Gyroscope);
                    await webHelper.SendSensorData<CompassData>(data.CompassData, SensorTypeEnum.Compass);
                }
                _cache.Add<MasterDataModel>(data, Config.CacheDataKey);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
