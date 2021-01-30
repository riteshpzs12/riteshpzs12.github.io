using System;
using Xamarin.Forms;

namespace SensorData.Views.CustomViews
{
    public class CustomEntry : Entry
    {
        public CustomEntry()
        {
            this.HorizontalTextAlignment = TextAlignment.Center;
            this.IsDirty = false;
            this.Keyboard = Keyboard.Chat;
            this.IsTextPredictionEnabled = false;
        }

        public static BindableProperty IsDirtyProperty = BindableProperty.Create(propertyName: "IsDirty", typeof(bool), typeof(CustomEntry), defaultValue: false, propertyChanged: IsDirtyPropertyChanged);

        public bool IsDirty
        {
            get { return (bool)base.GetValue(IsDirtyProperty); }
            set { base.SetValue(IsDirtyProperty, value); }
        }

        private static void IsDirtyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomEntry)bindable;
            control.IsDirty = (bool)newValue;
        }
    }
}
