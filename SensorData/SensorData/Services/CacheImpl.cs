using System;
using MonkeyCache.FileStore;
using SensorData.Models;

namespace SensorData.Services
{
    public class CacheImpl : ICache
    {
        public CacheImpl()
        {
            Barrel.ApplicationId = Config.ApplicationId;
        }

        public void Add<T>(T objects, string key)
        {
            Barrel.Current.Add<T>(key, objects, TimeSpan.MaxValue);
        }

        public T Get<T>(string key)
        {
            return Barrel.Current.Get<T>(key);
        }

        public void Remove(string key)
        {
            Barrel.Current.Empty(new string[] { key });
        }
    }
}
