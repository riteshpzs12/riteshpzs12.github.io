using System;
using SensorData.Models;

//Will deprecate this as shinysensor will be removed
namespace SensorData.Services
{
    public interface ISensorService
    {
        void StartCapture();
        void FlushData();
        MasterDataModel DisposeAll();
    }
}
