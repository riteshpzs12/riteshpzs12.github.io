using System;
using Android.Content;
using SensorData.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly:ExportRenderer(typeof(TabbedPage), typeof(TabPageRenderer))]
namespace SensorData.Droid.Renderer
{
    public class TabPageRenderer : TabbedPageRenderer
    {
        public TabPageRenderer(Context context) : base(context)
        {
            AutoPackage = false;
        }
    }
}
