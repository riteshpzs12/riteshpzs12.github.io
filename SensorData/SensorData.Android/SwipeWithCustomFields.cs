using System;
using System.ComponentModel;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using SensorData.Views.CustomViews;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(SensorData.Droid.SwipeWithCustomFields), nameof(SensorData.Droid.SwipeWithCustomFields))]
namespace SensorData.Droid
{
    public class SwipeWithCustomFields : PlatformEffect
    {
        private GestureDetectorCompat gestureRecognizer;
        private readonly InternalSwipeGestureDetector swipeDetector;
        private Command<TestTry> swipeWithPositionCommand;
        private DisplayMetrics displayMetrics;
		TestTry data;

        public SwipeWithCustomFields()
        {
			data = new TestTry()
			{
				Coordinate = new System.Collections.Generic.List<Point>()
			};
			swipeDetector = new InternalSwipeGestureDetector()
			{
				SwipeAction  = (CustomAndroidSwipeData obj) =>
				{
					var tap = swipeWithPositionCommand;
					if (tap != null)
					{
						data.Coordinate.Clear();
						data.Coordinate.Add(PxToDp(new Point(obj.start.GetX(), obj.start.GetY())));
						data.Coordinate.Add(PxToDp(new Point(obj.end.GetX(), obj.end.GetY())));
						data.CustomField1 = CalculateDirection(obj.start, obj.end);
						if (tap.CanExecute(data))
							tap.Execute(data);
					}
				},
			};
		}

        private string CalculateDirection(MotionEvent start, MotionEvent end)
        {
			var xdeviation = start.XPrecision - end.XPrecision;
			var ydeviation = start.YPrecision - end.YPrecision;
			if(Math.Abs(xdeviation)>Math.Abs(ydeviation))
            {
				if (xdeviation < 0)
					return "Right";
				else
					return "Left";
            }
			else
            {
				if (ydeviation < 0)
					return "Down";
				else
					return "Up";
			}
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			swipeWithPositionCommand = Gesture.GetSwipeCommand(Element);
		}

		private Point PxToDp(Point point)
		{
			point.X = point.X / displayMetrics.Density;
			point.Y = point.Y / displayMetrics.Density;
			return point;
		}

		protected override void OnAttached()
		{
			var control = Control ?? Container;

			var context = control.Context;
			displayMetrics = context.Resources.DisplayMetrics;
			//swipeDetector.Density = displayMetrics.Density;
			if (gestureRecognizer == null)
				gestureRecognizer = new GestureDetectorCompat(context, swipeDetector);
			control.Touch += ControlOnTouch;

			OnElementPropertyChanged(new PropertyChangedEventArgs(String.Empty));
		}

		private void ControlOnTouch(object sender, Android.Views.View.TouchEventArgs touchEventArgs)
		{
			gestureRecognizer?.OnTouchEvent(touchEventArgs.Event);
		}

		protected override void OnDetached()
		{
			var control = Control ?? Container;
			control.Touch -= ControlOnTouch;
		}
	}

	public class CustomAndroidSwipeData
    {
		public MotionEvent start { get; set; }
		public MotionEvent end { get; set; }
		public float xVelocity { get; set; }
		public float yVelocity { get; set; }
	}

    sealed class InternalSwipeGestureDetector : GestureDetector.SimpleOnGestureListener
    {
        public Action<CustomAndroidSwipeData> SwipeAction { get; set; }
        //public float Density { get; set; }

		public override bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
		{
			Console.WriteLine("OnFling");

			SwipeAction?.Invoke(new CustomAndroidSwipeData()
			{
				start = e1,
				end = e2,
				xVelocity = velocityX,
				yVelocity = velocityY
			});
			return base.OnFling(e1, e2, velocityX, velocityY);
		} 
	}
}
