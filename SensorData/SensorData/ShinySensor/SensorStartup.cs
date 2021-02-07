//using System;
//using System.Collections.Generic;
//using Microsoft.Extensions.DependencyInjection;
//using Shiny;

////Will deprecate this as shinysensor will be removed
//namespace SensorData.ShinySensor
//{
//    public class SensorStartup : ShinyStartup
//    {
//        public static Dictionary<string, bool> DeviceSensorsAvailable = new Dictionary<string, bool>();

//        public override void ConfigureServices(IServiceCollection services)
//        {
//            if (DeviceSensorsAvailable.ContainsKey("Accelerometer"))
//                DeviceSensorsAvailable["Accelerometer"] = services.UseAccelerometer();
//            else
//                DeviceSensorsAvailable.Add("Accelerometer", services.UseAccelerometer());

//            if (DeviceSensorsAvailable.ContainsKey("LightSensor"))
//                DeviceSensorsAvailable["LightSensor"] = services.UseAmbientLightSensor();
//            else
//                DeviceSensorsAvailable.Add("LightSensor", services.UseAmbientLightSensor());

//            if (DeviceSensorsAvailable.ContainsKey("Gyroscope"))
//                DeviceSensorsAvailable["Gyroscope"] = services.UseGyroscope();
//            else
//                DeviceSensorsAvailable.Add("Gyroscope", services.UseGyroscope());

//            if (DeviceSensorsAvailable.ContainsKey("Magnetometer"))
//                DeviceSensorsAvailable["Magnetometer"] = services.UseMagnetometer();
//            else
//                DeviceSensorsAvailable.Add("Magnetometer", services.UseMagnetometer());

//            if (DeviceSensorsAvailable.ContainsKey("Humidity"))
//                DeviceSensorsAvailable["Humidity"] = services.UseHumidity();
//            else
//                DeviceSensorsAvailable.Add("Humidity", services.UseHumidity());

//            if (DeviceSensorsAvailable.ContainsKey("Proximity"))
//                DeviceSensorsAvailable["Proximity"] = services.UseProximitySensor();
//            else
//                DeviceSensorsAvailable.Add("Proximity", services.UseProximitySensor());

//            if (DeviceSensorsAvailable.ContainsKey("HeartRate"))
//                DeviceSensorsAvailable["HeartRate"] = services.UseHeartRateMonitor();
//            else
//                DeviceSensorsAvailable.Add("HeartRate", services.UseHeartRateMonitor());

//            if (DeviceSensorsAvailable.ContainsKey("Barometer"))
//                DeviceSensorsAvailable["Barometer"] = services.UseBarometer();
//            else
//                DeviceSensorsAvailable.Add("Barometer", services.UseBarometer());

//            if (DeviceSensorsAvailable.ContainsKey("Compass"))
//                DeviceSensorsAvailable["Compass"] = services.UseCompass();
//            else
//                DeviceSensorsAvailable.Add("Compass", services.UseCompass());

//            if (DeviceSensorsAvailable.ContainsKey("Pedometer"))
//                DeviceSensorsAvailable["Pedometer"] = services.UsePedometer();
//            else
//                DeviceSensorsAvailable.Add("Pedometer", services.UsePedometer());

//            if (DeviceSensorsAvailable.ContainsKey("Temperature"))
//                DeviceSensorsAvailable["Temperature"] = services.UseTemperature();
//            else
//                DeviceSensorsAvailable.Add("Temperature", services.UseTemperature());            
//        }
//    }
//}
