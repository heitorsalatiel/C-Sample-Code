using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;

namespace Caching
{
    static class MyCache
    {
        public static MemoryCache _cache { get { return MemoryCache.Default;  } }

        public static T CacheCompleteFlow<T>(string cacheName, int cacheTime, Func<T> callback)
        {
            var obj = (T)GetItemCached(cacheName);

            if(obj == null)
            {
                obj = callback();
                SetItemCache(cacheName,obj, cacheTime);

            }

            return obj;
        }



        public static Object GetItemCached(string objectName)
        {
            return _cache.Get(objectName) as Object;
        }

        public static void SetItemCache(string objectName, object objectValue, int cacheTime)
        {
            var policy = new CacheItemPolicy() {
                SlidingExpiration = new TimeSpan(0,cacheTime,0)
            };

            _cache.Set(objectName, objectValue, policy);
        }
    }
}
