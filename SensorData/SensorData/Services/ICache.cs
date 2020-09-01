using System;
using SensorData.Models;

namespace SensorData.Services
{
    public interface ICache
    {
        void Add<T>(T objects, string key);
        T Get<T>(string key);
        void Remove(string key);
    }
}
