using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SensorData.Models;

namespace SensorData.Services
{
    public interface IWebHelper
    {
        Task<BaseResponse<LoginResponse>> PostLoginCall(CredModel data);

        Task SendSensorData<T>(Dictionary<long, T> data, SensorTypeEnum sensorTypeEnum);
    }
}
