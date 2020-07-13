using System;
using Android.App;
using Android.Runtime;

namespace SensorData.Droid
{
    [Application]
    public class MainApplication : Shiny.ShinyAndroidApplication<ShinySensor.SensorStartup>
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            Shiny.AndroidShinyHost.Init(
                this,
                new ShinySensor.SensorStartup()
            );
        }
    }
}
