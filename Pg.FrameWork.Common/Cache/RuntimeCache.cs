using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace Pg.FrameWork.Common.Cache
{
    /// <summary>
    /// 运行缓存
    /// </summary>
    public class RuntimeCache
    {
        private static readonly RuntimeCache InstanceTemp;

        private readonly ObjectCache _cache = MemoryCache.Default;

        public static RuntimeCache Instance
        {
            get
            {
                return InstanceTemp;
            }
        }

        static RuntimeCache()
        {
            InstanceTemp = new RuntimeCache();
        }

        private RuntimeCache()
        {
        }

        /// <summary>
        /// 根据键获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key) where T : class
        {
            return _cache[key] as T;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T">缓存对象</typeparam>
        /// <param name="key">键</param>
        /// <param name="t">缓存对象</param>
        /// <param name="limitMinute">缓存时间（分钟）</param>
        /// <returns></returns>
        public bool Set<T>(string key, T t, int limitMinute) where T : class
        {
            try
            {
                CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
                cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddMinutes(limitMinute);
                CacheItemPolicy policy = cacheItemPolicy;
                _cache.Set(key, t, policy);
            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 根据key，删除某个缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            try
            {
                _cache.Remove(key);
            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 删除所有缓存
        /// </summary>
        /// <returns></returns>
        public bool RemoveAll()
        {
            try
            {
                List<KeyValuePair<string, object>>.Enumerator enumerator = _cache.ToList().GetEnumerator();
                while (enumerator.MoveNext())
                {
                    _cache.Remove(enumerator.Current.Key);
                }
            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }
    }
}
