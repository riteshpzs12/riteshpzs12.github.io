using System.Threading.Tasks;
using Xamarin.Forms;

namespace SensorData.Services
{
    public class NavService : INavService
    {
        //public Application ApplicationContext { get; set; }

        public NavService()
        {
        }

        public void Goto(Page page)
        {
            var t = App.Current.MainPage.Navigation.NavigationStack.Count;
            if (t>0)
            {
                if (App.Current.MainPage.Navigation.NavigationStack[t-1] != page)
                    App.Current.MainPage.Navigation.PushAsync(new NavigationPage(page));
            }
        }

        public void OpenLandingPagePostLogin(Page page)
        {
            App.Current.MainPage = new NavigationPage(page);
        }

        public void ShowDialog(string title, string description)
        {
            if (App.Current.MainPage != null)
                App.Current.MainPage.DisplayAlert(title, description, "Ok");
        }

        public async Task<bool> ShowInteractiveDialogAsync(string title, string description, string positiveTetxt = "Yes", string negativText = "no")
        {
            if (App.Current.MainPage != null)
                return await App.Current.MainPage.DisplayAlert(title, description, positiveTetxt, negativText);
            return false;
        }
    }
}
