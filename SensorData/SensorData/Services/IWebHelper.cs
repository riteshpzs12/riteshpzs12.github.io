using System;
using System.Threading.Tasks;
using SensorData.Models;

namespace SensorData.Services
{
    public interface IWebHelper
    {
        Task<Object> PostCall(CustomeBaseRequest data);
    }
}
