using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SensorData.Services
{
    public class FileOperation : IFileOperation 
    {
        readonly IPlatformFileOp fileOp;

        public FileOperation()
        {
            fileOp = DependencyService.Get<IPlatformFileOp>();
        }

        public string GetData(string fileName)
        {
            return fileOp.GetData(fileName);
        }

        public async Task<string> Save(MemoryStream fileStream, string fileName)
        {
            return await fileOp.Save(fileStream, fileName);
        }
    }
}
