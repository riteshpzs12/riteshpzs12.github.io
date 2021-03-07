using SensorData.Views.CustomViews;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace SensorData.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LandingPages : Xamarin.Forms.TabbedPage
    {
        public LandingPages()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            On<Xamarin.Forms.PlatformConfiguration.Android>().SetIsSwipePagingEnabled(false);
            Children.Add(NavigationPageHelper.CreatePageForTabbedPage(new PrecisionPredictionTapPage(), "Tap It"));
            Children.Add(NavigationPageHelper.CreatePageForTabbedPage(new ScalePrecision(), "Slide It"));
        }
    }
}
