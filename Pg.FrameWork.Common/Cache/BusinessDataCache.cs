using System.Collections.Generic;

namespace Pg.FrameWork.Common.Cache
{
    public abstract class BusinessDataCache<TKey, TValue> : ICacheBase
    {
        private readonly SynchronizedCache<TKey, TValue> _cache;

        public IEnumerable<TValue> Values
        {
            get
            {
                return _cache.Values;
            }
        }

        public IEnumerable<TKey> Keys
        {
            get
            {
                return _cache.Keys;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                return _cache[key];
            }
        }

        protected BusinessDataCache()
        {
            _cache = new SynchronizedCache<TKey, TValue>();
        }

        public TValue Get(TKey key)
        {
            return _cache.GetValue(key);
        }

        public void Clear()
        {
            _cache.Clear();
        }

        public void Add(TKey key, TValue value)
        {
            _cache.Add(key, value);
        }

        public void Add(Dictionary<TKey, TValue> range)
        {
            _cache.AddRange(range);
        }

        public void Update(TKey key, TValue value)
        {
            _cache.Update(key, value);
        }

        public void Update(Dictionary<TKey, TValue> range)
        {
            _cache.UpdateRange(range);
        }

        public void Remove(TKey key)
        {
            _cache.Remove(key);
        }

        public void Remove(IEnumerable<TKey> range)
        {
            _cache.Remove(range);
        }
    }

}
