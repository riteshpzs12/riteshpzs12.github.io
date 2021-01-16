using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace SensorData.Views.CustomViews
{
    /// <summary>
    /// This view extends the Frame
    /// </summary>
    public class GestureFrame : Frame
    {
        public GestureFrame()
        {
        }        
    }

    public class TestTry
    {
        public List<Point> Coordinate { get; set; }
        public string Pressure { get; set; }
        public string CustomField1 { get; set; }
        public string CustomField2 { get; set; }
    }

    public static class Gesture
    {
        public static readonly BindableProperty TappedProperty = BindableProperty.CreateAttached("Tapped", typeof(Command<Point>), typeof(Gesture), null, propertyChanged: CommandChanged);

        public static Command<Point> GetCommand(BindableObject view)
        {
            return (Command<Point>)view.GetValue(TappedProperty);
        }

        public static void SetTapped(BindableObject view, Command<TestTry> value)
        {
            view.SetValue(TappedProperty, value);
        }

        public static readonly BindableProperty SwipedProperty = BindableProperty.CreateAttached("Swiped", typeof(Command<TestTry>), typeof(Gesture), null, propertyChanged: SwipeCommandChanged);

        public static Command<TestTry> GetSwipeCommand(BindableObject view)
        {
            return (Command<TestTry>)view.GetValue(SwipedProperty);
        }

        public static void SetSwiped(BindableObject view, Command<TestTry> value)
        {
            view.SetValue(SwipedProperty, value);
        }

        /// <summary>
        /// Attaches the Tap effect
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void CommandChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as View;
            if (view != null)
            {
                var effect = GetOrCreateEffect(view);
            }
        }

        /// <summary>
        /// Attaches the Swipe effect
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void SwipeCommandChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as View;
            if (view != null)
            {
                var effect = GetOrCreateSwipeEffect(view);
            }
        }

        /// <summary>
        /// Adds the swipe effect to the view if not attached
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        private static object GetOrCreateSwipeEffect(View view)
        {
            var effect = (SwipeGestureEffect)view.Effects.FirstOrDefault(e => e is SwipeGestureEffect);
            if (effect == null)
            {
                effect = new SwipeGestureEffect();
                view.Effects.Add(effect);
            }
            return effect;
        }

        /// <summary>
        /// Adds the tap effect to the view if not attached
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        private static GestureEffect GetOrCreateEffect(View view)
        {
            var effect = (GestureEffect)view.Effects.FirstOrDefault(e => e is GestureEffect);
            if (effect == null)
            {
                effect = new GestureEffect();
                view.Effects.Add(effect);
            }
            return effect;
        }

        /// <summary>
        /// The Tap Gesture effect
        /// </summary>
        class GestureEffect : RoutingEffect
        {
            // TapWithPositionGestureEffect is the Native description for the effect
            public GestureEffect() : base("SensorData.TapWithPositionGestureEffect")
            {
            }
        }

        /// <summary>
        /// The Swipe Gesture effect
        /// </summary>
        class SwipeGestureEffect : RoutingEffect
        {
            // SwipeWithCustomFields is the Native description for the effect
            public SwipeGestureEffect() : base("SensorData.SwipeWithCustomFields")
            {
            }
        }
    }
}
