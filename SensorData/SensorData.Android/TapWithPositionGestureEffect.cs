using System;
using System.ComponentModel;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using SensorData.Droid;
using SensorData.Views.CustomViews;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("SensorData")]
[assembly: ExportEffect(typeof(TapWithPositionGestureEffect), nameof(TapWithPositionGestureEffect))]

namespace SensorData.Droid
{
    public class TapWithPositionGestureEffect : PlatformEffect
    {
        private GestureDetectorCompat gestureRecognizer;
        private readonly InternalGestureDetector tapDetector;
        private Command<Point> tapWithPositionCommand;
        private DisplayMetrics displayMetrics;

        public TapWithPositionGestureEffect()
        {
            tapDetector = new InternalGestureDetector()
            {
                TapAction = (MotionEvent obj) =>
                {
                    var tap = tapWithPositionCommand;
                    if(tap!=null)
                    {
                        var point = PxToDp(new Point(obj.GetX(), obj.GetY()));
                        if (tap.CanExecute(point))
                            tap.Execute(point);
                    }
                },
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private Point PxToDp(Point point)
        {
            point.X = point.X / displayMetrics.Density;
            point.Y = point.Y / displayMetrics.Density;
            return point;
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            tapWithPositionCommand = Gesture.GetCommand(Element);
        }

        /// <summary>
        /// Attaches the touch events ascociated with a Tap
        /// called after the effect is attached to the view
        /// </summary>
        protected override void OnAttached()
        {
            var control = Control ?? Container;

            var context = control.Context;
            displayMetrics = context.Resources.DisplayMetrics;
            //tapDetector.Density = displayMetrics.Density;
            if (gestureRecognizer == null)
                gestureRecognizer = new GestureDetectorCompat(context, tapDetector);
            control.Touch += ControlOnTouch;

            OnElementPropertyChanged(new PropertyChangedEventArgs(String.Empty));
        }

        private void ControlOnTouch(object sender, Android.Views.View.TouchEventArgs touchEventArgs)
        {
            gestureRecognizer?.OnTouchEvent(touchEventArgs.Event);
        }

        /// <summary>
        /// Detaches the touch events ascociated with a Tap
        /// called after the effect is Detached to the view
        /// </summary>
        protected override void OnDetached()
        {
            var control = Control ?? Container;
            control.Touch -= ControlOnTouch;
        }
    }

    sealed class InternalGestureDetector : GestureDetector.SimpleOnGestureListener
    {
        public Action<MotionEvent> TapAction { get; set; }
        //public float Density { get; set; }

        /// <summary>
        /// Reads a tap using the native listener
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool OnSingleTapUp(MotionEvent e)
        {
            TapAction?.Invoke(e);
            return true;
        }
    }
}
