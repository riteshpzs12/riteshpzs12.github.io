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

        public void Add<T>(T objects)
        {
            Barrel.Current.Add<T>(Config.CredCacheKey, objects, TimeSpan.MaxValue);
        }

        public T Get<T>(string key)
        {
            return Barrel.Current.Get<T>(key);
        }
    }
}
