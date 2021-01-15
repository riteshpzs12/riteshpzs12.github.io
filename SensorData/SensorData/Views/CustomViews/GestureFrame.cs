using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace SensorData.Views.CustomViews
{
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

        private static void CommandChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as View;
            if (view != null)
            {
                var effect = GetOrCreateEffect(view);
            }
        }

        private static void SwipeCommandChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as View;
            if (view != null)
            {
                var effect = GetOrCreateSwipeEffect(view);
            }
        }

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

        class GestureEffect : RoutingEffect
        {
            public GestureEffect() : base("SensorData.TapWithPositionGestureEffect")
            {
            }
        }

        class SwipeGestureEffect : RoutingEffect
        {
            public SwipeGestureEffect() : base("SensorData.SwipeWithCustomFields")
            {
            }
        }
    }
}
