using System;
using SensorData.ContainerHelper;
using SensorData.ViewModel.Registrations;
using SensorData.Views.CustomViews;
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
                button = consent.Width;
            if (Device.RuntimePlatform == Device.Android)
                List.HeightRequest = ((ViewModel.ListItems.Count + 1) * 75) + 50;
            else
                List.HeightRequest = ((ViewModel.ListItems.Count + 1) * 75) + 20;
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
            entry.BackgroundColor = Color.White;
        }

        void Entry_UnFocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            Entry entry = (Entry)sender;
            ((CustomEntry)entry).IsDirty = true;
            var parent = (Frame)(entry.Parent);
            if (entry != null && parent != null)
            {
                Animate(entry, false, 200);
                Animate(parent, false, 200);
            }
            entry.Unfocus();
            var IsValid = ViewModel.validate();

            _ = Name.IsDirty ? IsValid[0] ? VisualStateManager.GoToState(Name, "Valid") : VisualStateManager.GoToState(Name, "Invalid") : VisualStateManager.GoToState(Name, "Normal");
            _ = Email.IsDirty ? IsValid[1] ? VisualStateManager.GoToState(Email, "Valid") : VisualStateManager.GoToState(Email, "Invalid") : VisualStateManager.GoToState(Email, "Normal");
            _ = Password1.IsDirty ? IsValid[2] ? VisualStateManager.GoToState(Password1, "Valid") : VisualStateManager.GoToState(Password1, "Invalid") : VisualStateManager.GoToState(Password1, "Normal") ;
            _ = Password2.IsDirty ? IsValid[3] ? VisualStateManager.GoToState(Password2, "Valid") : VisualStateManager.GoToState(Password2, "Invalid") : VisualStateManager.GoToState(Password2, "Normal");

            if (IsValid[0] && IsValid[1] && IsValid[2] && IsValid[3])
                Policy.IsEnabled = true;
            else
            {
                Policy.IsChecked = false;
                Policy.IsEnabled = false;
            }
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

        void consent_Clicked(System.Object sender, EventArgs eventArgs)
        {
            consent.IsVisible = false;
            register.IsVisible = true;
            ViewModel.ShowConsent();
        }

        void CheckBox_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked)
            {
                register.IsEnabled = true;
                var animation = new Animation(v => register.WidthRequest = v, button, button * 2);
                animation.Commit(this, "Enable", length: 200, easing: Easing.Linear,
                    finished: (v, c) => { consent.WidthRequest = button * 2; consent.IsEnabled = true; }, repeat: () => false);
                ViewModel.ResolvePermissions();
            }                
            else
            {
                register.IsEnabled = false;
                var animation = new Animation(v => register.WidthRequest = v, button * 2, button);
                animation.Commit(this, "Enable", length: 200, easing: Easing.Linear,
                    finished: (v, c) => { consent.WidthRequest = button; consent.IsEnabled = false; }, repeat: () => false);
            }
        }
    }
}
