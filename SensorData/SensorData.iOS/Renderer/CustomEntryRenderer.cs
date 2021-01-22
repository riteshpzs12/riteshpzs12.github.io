using System;
using SensorData.iOS.Renderer;
using SensorData.Views.CustomViews;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace SensorData.iOS.Renderer
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if(Control != null)
            {
                Control.BorderStyle = UIKit.UITextBorderStyle.None;
                Control.Layer.BorderWidth = 0;
            }
        }
    }
}
