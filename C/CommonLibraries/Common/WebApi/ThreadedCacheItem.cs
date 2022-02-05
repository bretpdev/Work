using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Uheaa.Common.WebApi
{
    public class ThreadedCacheItem<T> where T : class
    {
        private readonly string key;
        private readonly string timeKey;
        private readonly Func<T> generateCacheItem;
        private readonly int expirationInMinutes;

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
                T cachedItem = HttpRuntime.Cache[key] as T;
                var itemInsertedAt = HttpRuntime.Cache[timeKey] as DateTime?;
                var timeInCache = DateTime.Now - itemInsertedAt;
                if (timeInCache?.TotalMinutes >= expirationInMinutes)
                    return GenerateCacheItem();
                return cachedItem;
            }
        }

        private T GenerateCacheItem()
        {
            var cachedItem = generateCacheItem();
            HttpRuntime.Cache.Insert(key, cachedItem);
            HttpRuntime.Cache.Insert(timeKey, DateTime.Now);
            return cachedItem;
        }
    }

}
