using System;
using SensorData.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:ExportRenderer(typeof(TabbedPage), typeof(TabPageRenderer))]
namespace SensorData.iOS.Renderer
{
    public class TabPageRenderer : TabbedRenderer
    {
        
    }
}
