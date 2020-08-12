using System;
using System.Threading.Tasks;
using SensorData.Models;

namespace SensorData.Services
{
    public interface IWebHelper
    {
        Task<bool> GetCall(CustomeBaseRequest data);
    }
}
