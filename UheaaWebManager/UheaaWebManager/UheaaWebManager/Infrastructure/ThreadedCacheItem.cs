using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace UheaaWebManager
{
    public class ThreadedCacheItem<T> where T : class
    {
        private string key;
        private string timeKey;
        Func<T> generateCacheItem;
        Task cacheGenerationInProgress;
        int expirationInMinutes;
        
        public ThreadedCacheItem(string key, int expirationInMinutes, Func<T> generateCacheItem)
        {
            this.key = key;
            this.timeKey = key + "_inserted_at";
            this.generateCacheItem = generateCacheItem;
            this.expirationInMinutes = expirationInMinutes;
            GenerateCacheItem();
        }

        public T Item
        {
            get
            {
                T cachedItem = null;
                while ((cachedItem = HttpRuntime.Cache[key] as T) == null)
                    Thread.Sleep(1000);
                var itemInsertedAt = HttpRuntime.Cache[timeKey] as DateTime?;
                var timeInCache = itemInsertedAt.Value - DateTime.Now;
                if (timeInCache.TotalMinutes >= expirationInMinutes)
                    generateCacheItem();
                return cachedItem;
            }
        }

        private void GenerateCacheItem()
        {
            cacheGenerationInProgress = Task.Run(() =>
            {
                var cachedItem = generateCacheItem();
                HttpRuntime.Cache.Insert(key, cachedItem);
                HttpRuntime.Cache.Insert(timeKey, DateTime.Now);
            });
        }
    }
}