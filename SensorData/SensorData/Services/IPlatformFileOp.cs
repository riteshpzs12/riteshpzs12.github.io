using System;
using System.IO;
using System.Threading.Tasks;

namespace SensorData.Services
{
    public interface IPlatformFileOp
    {
        Task<string> Save(MemoryStream fileStream, string fileName);
        string Save(string[] fileStream, string fileName);
        string GetData(string fileName);
        bool CheckFileExistence(string fileName);
        void DeleteFile(string fileName);
        void ShowFileFromLocal(string fileName);
    }
}
