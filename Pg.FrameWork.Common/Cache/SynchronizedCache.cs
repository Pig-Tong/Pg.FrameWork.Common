using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Pg.FrameWork.Common.Cache
{
    /// <summary>
    /// 同步缓存抽象类
    /// </summary>
    public abstract class SynchronizedCache
    {
        /// <summary>
        /// 缓存锁关键字
        /// </summary>
        private readonly ReaderWriterLockSlim _cacheLock;

        /// <summary>
        /// 构造函数
        /// </summary>
        protected SynchronizedCache()
        {
            _cacheLock = new ReaderWriterLockSlim();
        }

        /// <summary>
        /// 进入读锁状态
        /// </summary>
        public void EnterReadLock()
        {
            _cacheLock.EnterReadLock();
        }

        /// <summary>
        /// 退出读锁状态
        /// </summary>
        public void ExitReadLock()
        {
            _cacheLock.ExitReadLock();
        }

        /// <summary>
        /// 进入写锁状态
        /// </summary>
        public void EnterWriteLock()
        {
            _cacheLock.EnterWriteLock();
        }

        /// <summary>
        /// 退出写锁状态
        /// </summary>
        public void ExitWriteLock()
        {
            _cacheLock.ExitWriteLock();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class SynchronizedCache<TKey, TValue> : SynchronizedCache
    {
        private readonly Dictionary<TKey, TValue> _dataList;

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
                EnterReadLock();
                try
                {
                    Dictionary<TKey, TValue>.ValueCollection values = _dataList.Values;
                    return from item in values
                        select (item);
                }
                finally
                {
                    ExitReadLock();
                }
            }
        }

        public IEnumerable<TKey> Keys
        {
            get
            {
                EnterReadLock();
                try
                {
                    return _dataList.Keys.ToArray();
                }
                finally
                {
                    ExitReadLock();
                }
            }
        }

        public SynchronizedCache()
        {
            _dataList = new Dictionary<TKey, TValue>();
        }

        public TValue GetValue(TKey key)
        {
            TValue result = default(TValue);
            EnterReadLock();
            try
            {
                if (_dataList.ContainsKey(key))
                {
                    return _dataList[key];
                }
                return result;
            }
            finally
            {
                ExitReadLock();
            }
        }

        public void Clear()
        {
            EnterWriteLock();
            try
            {
                _dataList.Clear();
            }
            finally
            {
                ExitWriteLock();
            }
        }

        public void Refresh(Dictionary<TKey, TValue> range)
        {
            EnterWriteLock();
            try
            {
                _dataList.Clear();
                if (range != null)
                {
                    AddRanges(range);
                }
            }
            finally
            {
                ExitWriteLock();
            }
        }

        public void Add(TKey key, TValue value)
        {
            EnterWriteLock();
            try
            {
                AddItem(key, value);
            }
            finally
            {
                ExitWriteLock();
            }
        }

        public void AddRange(Dictionary<TKey, TValue> range)
        {
            EnterWriteLock();
            try
            {
                AddRanges(range);
            }
            finally
            {
                ExitWriteLock();
            }
        }

        public void Remove(TKey key)
        {
            EnterWriteLock();
            try
            {
                RemoveItem(key);
            }
            finally
            {
                ExitWriteLock();
            }
        }

        public void Remove(IEnumerable<TKey> range)
        {
            EnterWriteLock();
            try
            {
                RemoveRanges(range);
            }
            finally
            {
                ExitWriteLock();
            }
        }

        public void Update(TKey key, TValue value)
        {
            EnterWriteLock();
            try
            {
                UpdateItem(key, value);
            }
            finally
            {
                ExitWriteLock();
            }
        }

        public void UpdateRange(Dictionary<TKey, TValue> range)
        {
            EnterWriteLock();
            try
            {
                UpdateRanges(range);
            }
            finally
            {
                ExitWriteLock();
            }
        }

        public bool ContainsKey(TKey key)
        {
            return _dataList.ContainsKey(key);
        }

        private void AddRanges(Dictionary<TKey, TValue> range)
        {
            if (range != null)
            {
                foreach (KeyValuePair<TKey, TValue> item in range)
                {
                    AddItem(item.Key, item.Value);
                }
            }
        }

        private void AddItem(TKey key, TValue value)
        {
            if (ContainsKey(key))
            {
                _dataList[key] = value;
            }
            else
            {
                _dataList.Add(key, value);
            }
        }

        private void RemoveRanges(IEnumerable<TKey> range)
        {
            if (range != null)
            {
                foreach (TKey item in range)
                {
                    RemoveItem(item);
                }
            }
        }

        private void RemoveItem(TKey key)
        {
            if (ContainsKey(key))
            {
                _dataList.Remove(key);
            }
        }

        private void UpdateRanges(Dictionary<TKey, TValue> range)
        {
            if (range != null)
            {
                foreach (KeyValuePair<TKey, TValue> item in range)
                {
                    UpdateItem(item.Key, item.Value);
                }
            }
        }

        private void UpdateItem(TKey key, TValue value)
        {
            AddItem(key, value);
        }
    }
}
