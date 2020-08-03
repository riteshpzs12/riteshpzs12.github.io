using System.Threading.Tasks;
using Xamarin.Forms;

namespace SensorData.Services
{
    public interface INavService
    {
        void Goto(Page page);
        void ShowDialog(string title, string description);
        Task<bool> ShowInteractiveDialogAsync(string title, string description, string positiveTetxt = "Yes", string negativText = "no");
    }
}
