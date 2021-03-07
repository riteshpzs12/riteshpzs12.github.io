using System;
using Xamarin.Forms;

namespace SensorData.Views.CustomViews
{
    public class NavigationPageHelper
    {
        public NavigationPageHelper()
        {
        }

        public static NavigationPage CreatePageForTabbedPage(Page page, string Title)
        {
            return new NavigationPage(page)
            {
                Title = Title
            };
        }
    }
}
