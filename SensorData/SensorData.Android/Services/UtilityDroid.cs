using System;
using Android.OS;
using SensorData.Droid.Services;
using SensorData.Services;
using static Android.Provider.Settings;

[assembly: Xamarin.Forms.Dependency(typeof(UtilityDroid))]
namespace SensorData.Droid.Services
{
    public class UtilityDroid : IUtility
    {
        public UtilityDroid()
        {
        }

        public void GetAppId()
        {
            if (string.IsNullOrEmpty(App.DeviceId))
            {
                string id = Android.OS.Build.GetSerial();
                if (string.IsNullOrWhiteSpace(id) || id == Build.Unknown || id == "0")
                {
                    try
                    {
                        var context = Android.App.Application.Context;
                        id = Secure.GetString(context.ContentResolver, Secure.AndroidId);
                    }
                    catch (Exception ex)
                    {
                        id = "UNKNOWN";
                    }
                }
                App.DeviceId = id;
            }
        }
    }
}
