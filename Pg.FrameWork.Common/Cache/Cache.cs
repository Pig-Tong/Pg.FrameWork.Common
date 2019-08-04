using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Pg.FrameWork.Common.Cache
{
    public abstract class Cache
    {
        private const int LockApplyTimeOut = 10000;

        internal readonly ReaderWriterLock Locker;

        protected double? Timeout
        {
            get;
            set;
        }

        protected Cache()
        {
            Locker = new ReaderWriterLock();
        }

        protected bool IsExpired(DateTime lastUpdateTime)
        {
            if (Timeout.HasValue)
            {
                return (DateTime.Now - lastUpdateTime).TotalSeconds > Timeout.Value;
            }
            return false;
        }

        protected void AcquireWriterLock()
        {
            Locker.AcquireWriterLock(10000);
        }

        protected void ReleaseWriterLock()
        {
            Locker.ReleaseWriterLock();
        }

        protected void AcquireReaderLock()
        {
            Locker.AcquireReaderLock(10000);
        }

        protected void ReleaseReaderLock()
        {
            Locker.ReleaseReaderLock();
        }
    }

    public class Cache<TValue> : Cache
    {
        private CacheItem<TValue> _mvalue;

        public TValue Value
        {
            get
            {
                TValue result = default(TValue);
                AcquireWriterLock();
                if (_mvalue != null && !IsExpired(_mvalue.Time))
                {
                    result = _mvalue.Value;
                }
                ReleaseWriterLock();
                return result;
            }
            set
            {
                AcquireReaderLock();
                if (_mvalue == null)
                {
                    _mvalue = new CacheItem<TValue>(value);
                }
                else
                {
                    _mvalue.Value = value;
                }
                ReleaseReaderLock();
            }
        }
    }

    public class Cache<TKey, TValue> : Cache
    {
        private readonly Dictionary<TKey, CacheItem<TValue>> _dataList;

        public TValue this[TKey key]
        {
            get
            {
                return GetValue(key);
            }
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                AcquireReaderLock();
                Dictionary<TKey, CacheItem<TValue>>.ValueCollection values = _dataList.Values;
                ReleaseReaderLock();
                return from item in values
                    where !IsExpired(item.Time)
                    select item.Value;
            }
        }

        public Cache()
        {
            _dataList = new Dictionary<TKey, CacheItem<TValue>>();
        }

        public void Add(TKey key, TValue value)
        {
            AcquireWriterLock();
            AddItem(key, value);
            ReleaseWriterLock();
        }

        public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> range)
        {
            if (range != null)
            {
                AcquireWriterLock();
                AddRanges(range);
                ReleaseWriterLock();
            }
        }

        public void Refresh(IEnumerable<KeyValuePair<TKey, TValue>> range)
        {
            AcquireWriterLock();
            _dataList.Clear();
            if (range != null)
            {
                AddRanges(range);
            }
            ReleaseWriterLock();
        }

        public TValue GetValue(TKey key)
        {
            TValue result = default(TValue);
            AcquireReaderLock();
            if (_dataList.ContainsKey(key))
            {
                CacheItem<TValue> cacheItem = _dataList[key];
                if (!IsExpired(cacheItem.Time))
                {
                    result = cacheItem.Value;
                }
            }
            ReleaseReaderLock();
            return result;
        }

        public void Update(TKey key, Func<TValue, bool> updateFunc)
        {
            if (updateFunc != null)
            {
                AcquireWriterLock();
                if (_dataList.ContainsKey(key))
                {
                    updateFunc(_dataList[key].Value);
                }
                ReleaseWriterLock();
            }
        }

        public void Update(TKey key, Func<TValue> insertFunc, Func<TValue, bool> updateFunc)
        {
            AcquireWriterLock();
            if (_dataList.ContainsKey(key))
            {
                if (updateFunc != null)
                {
                    updateFunc(_dataList[key].Value);
                }
            }
            else if (insertFunc != null)
            {
                _dataList.Add(key, new CacheItem<TValue>(insertFunc()));
            }
            ReleaseWriterLock();
        }

        public void Update(TKey key, TValue value)
        {
            AcquireWriterLock();
            if (_dataList.ContainsKey(key))
            {
                _dataList[key].Value = value;
            }
            else
            {
                _dataList.Add(key, new CacheItem<TValue>(value));
            }
            ReleaseWriterLock();
        }

        public void Remove(TKey key)
        {
            AcquireWriterLock();
            if (_dataList.ContainsKey(key))
            {
                _dataList.Remove(key);
            }
            ReleaseWriterLock();
        }

        public void Clear()
        {
            AcquireWriterLock();
            _dataList.Clear();
            ReleaseWriterLock();
        }

        private void AddRanges(IEnumerable<KeyValuePair<TKey, TValue>> range)
        {
            foreach (KeyValuePair<TKey, TValue> item in range)
            {
                AddItem(item.Key, item.Value);
            }
        }

        private void AddItem(TKey key, TValue value)
        {
            if (_dataList.ContainsKey(key))
            {
                _dataList[key].Value = value;
            }
            else
            {
                _dataList.Add(key, new CacheItem<TValue>(value));
            }
        }
    }
}
