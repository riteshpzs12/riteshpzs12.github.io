using System;
using Android.Content;
using Android.Graphics.Drawables;
using SensorData.Droid.Renderer;
using SensorData.Views.CustomViews;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace SensorData.Droid.Renderer
{
    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(context)
        {
            AutoPackage = false;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if(Control != null)
            {
                Control.Background = new ColorDrawable(Android.Graphics.Color.Transparent);
            }
        }
    }
}
