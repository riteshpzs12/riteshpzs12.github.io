using System;
using SensorData.iOS.Services;
using SensorData.Services;

[assembly: Xamarin.Forms.Dependency(typeof(UtilityIOS))]
namespace SensorData.iOS.Services
{
    public class UtilityIOS : IUtility
    {
        public UtilityIOS()
        {
        }

        public void GetAppId()
        {
            if(string.IsNullOrEmpty(App.DeviceId))
                App.DeviceId = UIKit.UIDevice.CurrentDevice.IdentifierForVendor.AsString();
        }
    }
}
