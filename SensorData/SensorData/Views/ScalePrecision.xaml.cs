using SensorData.ContainerHelper;
using SensorData.ViewModel.ScalePrecision;
using Xamarin.Forms;

namespace SensorData.Views
{
    public partial class ScalePrecision : ContentPage
    {
        public ScalePrecision()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            IIoCContainer container = Builder.Build();
            ViewModel = container.Resolve<IBaseViewModel>() as ScalePrecisionViewModel;
        }

        public ScalePrecisionViewModel ViewModel
        {
            get { return BindingContext as ScalePrecisionViewModel; }
            set { BindingContext = value; }
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            ViewModel.FrameBound = CheckParent.Width - 20;
            ViewModel.UpdatePlayGround();
            //ViewModel.YTop = CheckParent.Height;
            //ViewModel.XPlay = PlayTest.Width;
            //ViewModel.YPlay = PlayTest.Height;
            //ViewModel.UpdateDotLocation();
        }

        void slider_ValueChanged(System.Object sender, Xamarin.Forms.ValueChangedEventArgs e)
        {
            ViewModel.SlideIt(e.NewValue);
        }
    }
}
