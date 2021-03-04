using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using static Android.Provider.Settings;

namespace SensorData.Droid
{
    [Application]
    public class MainApplication : Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
        }
    }
}
