using System;
using SensorData.Models;

namespace SensorData.Services
{
    public interface ICache
    {
        void Add<T>(T objects);
        T Get<T>(string key);
    }
}
