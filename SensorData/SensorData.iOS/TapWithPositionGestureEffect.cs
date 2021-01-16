using System;
using System.ComponentModel;
using SensorData.Views.CustomViews;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("SensorData")]
[assembly: ExportEffect(typeof(SensorData.iOS.TapWithPositionGestureEffect), nameof(SensorData.iOS.TapWithPositionGestureEffect))]
namespace SensorData.iOS
{
    internal class TapWithPositionGestureEffect : PlatformEffect
    {
        private readonly UITapGestureRecognizer tapDetector;
        private Command<Point> tapWithPositionCommand;

        public TapWithPositionGestureEffect()
        {
            tapDetector = CreateTapRecognizer(() => tapWithPositionCommand); ;
        }

        /// <summary>
        /// Creates the tap recognizer/listener
        /// </summary>
        /// <param name="getCommand"></param>
        /// <returns></returns>
        private UITapGestureRecognizer CreateTapRecognizer(Func<Command<Point>> getCommand)
        {
            return new UITapGestureRecognizer(() =>
            {
                var handler = getCommand();
                if (handler != null)
                {
                    var control = Control ?? Container;
                    var tapPoint = tapDetector.LocationInView(control);
                    var point = new Point(tapPoint.X, tapPoint.Y);
                    if (handler.CanExecute(point) == true)
                        handler.Execute(point);
                }
            })
            {
                Enabled = false,
                ShouldRecognizeSimultaneously = (recognizer, gestureRecognizer) => true,
            };
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            tapWithPositionCommand = Gesture.GetCommand(Element);
        }

        /// <summary>
        /// Adds the recognizer/listener to the view
        /// </summary>
        protected override void OnAttached()
        {
            var control = Control ?? Container;

            control.AddGestureRecognizer(tapDetector);
            tapDetector.Enabled = true;
            control.UserInteractionEnabled = true;
            OnElementPropertyChanged(new PropertyChangedEventArgs(String.Empty));
        }

        /// <summary>
        /// Removes the recognizer/listener to the view
        /// </summary>
        protected override void OnDetached()
        {
            var control = Control ?? Container;
            tapDetector.Enabled = false;
            control.UserInteractionEnabled = true;
            control.RemoveGestureRecognizer(tapDetector);
        }
    }
}
