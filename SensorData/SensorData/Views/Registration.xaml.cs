using System;
using System.Collections.Generic;
using SensorData.ContainerHelper;
using SensorData.ViewModel.Registrations;
using Xamarin.Forms;

namespace SensorData.Views
{
    public partial class Registration : ContentPage
    {
        double container = 0.0;
        double button = 0.0;
        public Registration()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            IIoCContainer container = Builder.Build();
            ViewModel = container.Resolve<IBaseViewModel>() as RegistrationViewModel;
        }

        public RegistrationViewModel ViewModel
        {
            get { return BindingContext as RegistrationViewModel; }
            set { BindingContext = value; }
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            App.Current.MainPage.DisplayAlert("Registration", "User Registered Successfully", "Ok");
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (container == 0.0)
            {
                container = Container.Width;
                Name.WidthRequest = container - 120;
                Email.WidthRequest = container - 120;
                Password1.WidthRequest = container - 120;
                Password2.WidthRequest = container - 120;
            }
            if (button == 0.0)
                button = register.Width;
        }

        void Entry_Focused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            Entry entry = (Entry)sender;
            var parent = (Frame)(entry.Parent);
            if (entry != null && parent != null)
            {
                Animate(entry, true, 200);
                Animate(parent, true, 200);
            }
            entry.Focus();
        }

        void Entry_UnFocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            Entry entry = (Entry)sender;
            var parent = (Frame)(entry.Parent);
            if (entry != null && parent != null)
            {
                Animate(entry, false, 200);
                Animate(parent, false, 200);
            }
            entry.Unfocus();
            var IsValid = ViewModel.validate();
            _ = IsValid[0] ? VisualStateManager.GoToState(Name, "Normal") : VisualStateManager.GoToState(Name, "Invalid");
            _ = IsValid[1] ? VisualStateManager.GoToState(Email, "Normal") : VisualStateManager.GoToState(Email, "Invalid");
            _ = IsValid[2] ? VisualStateManager.GoToState(Password1, "Normal") : VisualStateManager.GoToState(Password1, "Invalid");
            _ = IsValid[3] ? VisualStateManager.GoToState(Password2, "Normal") : VisualStateManager.GoToState(Password2, "Invalid"); 
            if (!IsValid[0] && IsValid[1] && IsValid[2] && IsValid[3])
                Policy.IsEnabled = true;
        }

        public void Animate(VisualElement view, bool scaleFlag, uint length)
        {
            string name = "FocusIn";
            double start = view.Width;
            double end = container - 40;
            if (!scaleFlag)
            {
                name = "FocusOut";
                end = container - 100;
            }
            if(view != null)
            {
                var animation = new Animation(v => view.WidthRequest = v, start, end);
                animation.Commit(this, name, length: length, easing: Easing.Linear,
                    finished: (v, c) => view.WidthRequest = end, repeat: () => false);
            }
        }

        void CheckBox_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked)
            {
                register.IsEnabled = true;
                var animation = new Animation(v => register.WidthRequest = v, button, button * 2);
                animation.Commit(this, "Enable", length: 200, easing: Easing.Linear,
                    finished: (v, c) => { register.WidthRequest = button * 2;}, repeat: () => false);
            }                
            else
            {
                register.IsEnabled = false;
                var animation = new Animation(v => register.WidthRequest = v, button * 2, button);
                animation.Commit(this, "Enable", length: 200, easing: Easing.Linear,
                    finished: (v, c) => { register.WidthRequest = button; }, repeat: () => false);
            }
        }
    }
}
