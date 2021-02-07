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
        public const string CacheDataKey = "SensorSessionData_";

        public const string DataPushUrl = "BaseAddress/";
        public const string LoginUrl = "http://ec2-13-235-74-0.ap-south-1.compute.amazonaws.com:3000/login";
    }
}
