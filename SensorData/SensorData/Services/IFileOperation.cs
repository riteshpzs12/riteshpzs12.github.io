using System.IO;
using System.Threading.Tasks;

namespace SensorData.Services
{
    public interface IFileOperation
    {
        Task<string> Save(MemoryStream fileStream, string fileName);
        string GetData(string fileName);
    }
}
