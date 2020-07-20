using System;
using SensorData.Models;

namespace SensorData.Services
{
    public interface ISensorService
    {
        void StartCapture();
        void FlushData();
        MasterDataModel DisposeAll();
    }
}
