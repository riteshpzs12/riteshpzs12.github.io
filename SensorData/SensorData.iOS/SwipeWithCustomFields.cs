using System;
using System.Collections.Generic;
using System.ComponentModel;
using SensorData.Views.CustomViews;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

//[assembly: ResolutionGroupName("SensorData")]
[assembly: ExportEffect(typeof(SensorData.iOS.SwipeWithCustomFields), nameof(SensorData.iOS.SwipeWithCustomFields))]
namespace SensorData.iOS
{
    internal class SwipeWithCustomFields : PlatformEffect
    {
        private readonly List<UISwipeGestureRecognizer> swipeDetectors;
        private Command<TestTry> swipeWithCustomdataCommand;
        public TestTry data;

        public SwipeWithCustomFields()
        {
            data = new TestTry()
            {
                Coordinate = new List<Point>()
            };
            swipeDetectors = CreateSwipeRecognizer(() => swipeWithCustomdataCommand); ;
        }

        /// <summary>
		/// Attaches the listeners/recognizers ascociated with a Swipe to the view
		/// called after the effect is attached to the view
		/// </summary>
        protected override void OnAttached()
        {
            var control = Control ?? Container;

            swipeDetectors.ForEach((a) => control.AddGestureRecognizer(a));
            swipeDetectors.ForEach((a) => a.Enabled = true);
            control.UserInteractionEnabled = true;
            OnElementPropertyChanged(new PropertyChangedEventArgs(String.Empty));
        }

        /// <summary>
		/// Detaches the listeners/recognizers ascociated with a Swipe
		/// called after the effect is detached to the view
		/// </summary>
        protected override void OnDetached()
        {
            var control = Control ?? Container;
            swipeDetectors.ForEach((a) => control.RemoveGestureRecognizer(a));
            swipeDetectors.ForEach((a) => a.Enabled = false);
            control.UserInteractionEnabled = true;
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            swipeWithCustomdataCommand = Gesture.GetSwipeCommand(Element);
        }

        /// <summary>
        /// Creates list of swipe listeners/recognizers for different directions
        /// </summary>
        /// <param name="getCommand"></param>
        /// <returns></returns>
        private List<UISwipeGestureRecognizer> CreateSwipeRecognizer(Func<Command<TestTry>> getCommand)
        {
            UISwipeGestureRecognizer left = TestCustom(UISwipeGestureRecognizerDirection.Left, getCommand);
            UISwipeGestureRecognizer right = TestCustom(UISwipeGestureRecognizerDirection.Right, getCommand);
            UISwipeGestureRecognizer up = TestCustom(UISwipeGestureRecognizerDirection.Up, getCommand);
            UISwipeGestureRecognizer down = TestCustom(UISwipeGestureRecognizerDirection.Down, getCommand);
            var res = new List<UISwipeGestureRecognizer>();
            res.Add(left);res.Add(right);res.Add(up);
            res.Add(down);
            return res;
        }

        /// <summary>
        /// Creates each listener/recognizer with custom logic
        /// </summary>
        /// <param name="uISwipeGestureRecognizerDirection"></param>
        /// <param name="getCommand"></param>
        /// <returns></returns>
        private UISwipeGestureRecognizer TestCustom(UISwipeGestureRecognizerDirection uISwipeGestureRecognizerDirection, Func<Command<TestTry>> getCommand)
        {
            return new UISwipeGestureRecognizer(() =>
            {
                var handler = getCommand();
                if (handler != null)
                {
                    var control = Control ?? Container;
                    var tapPoint = swipeDetectors[0].LocationInView(control);
                    data.CustomField1 = uISwipeGestureRecognizerDirection.ToString();
                    data.Coordinate.Clear();
                    data.Coordinate.Add(new Point(tapPoint.X, tapPoint.Y));
                    if (handler.CanExecute(data) == true)
                        handler.Execute(data);
                }
            })
            {
                Enabled = false,
                ShouldRecognizeSimultaneously = (recognizer, gestureRecognizer) => true,
                Direction = uISwipeGestureRecognizerDirection
            };
        }
    }
}
