using System;
using Xamarin.Essentials;

namespace SensorData
{
    //This file will contain the config urls and the constants for the project
    public class Config
    {
        public const SensorSpeed sensorSpeed = SensorSpeed.Fastest;
        public const string ApplicationId = "Sensor.Data.Initial";
        public const string CredCacheKey = "CredentialKey";
        public const string InstallationToken = "InstallToken";
        public const string StillFileName = "Test_Data_1.txt";
        public const string UsageFileName = "Test_Data_2.txt";
        public const string CacheDataKey = "SensorSessionData";
        public const string SessionDataKey = "CacheSessionKey";

        public const string DataPushUrl = "http://192.168.0.112:5000/api/data/";
        public const string RegisterUrl = "http://192.168.0.112:5000/api/user/register";
        public const string LoginUrl = "http://ec2-13-235-74-0.ap-south-1.compute.amazonaws.com:3000/login";
    }
}
