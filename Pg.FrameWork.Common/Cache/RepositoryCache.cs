using System.Collections.Generic;
using System.Timers;

namespace Pg.FrameWork.Common.Cache
{
    public class RepositoryCache<TKey, TValue>
    {
        public interface IRepository
        {
            IEnumerable<KeyValuePair<TKey, TValue>> Query();

            object Insert(TValue value);

            int Modify(TValue value);

            void Delete(TValue value);
        }

        private const double DefaultInterval = 120000.0;

        private readonly IRepository _repository;

        private readonly Cache<TKey, TValue> _cache;

        private readonly Timer timer;

        public IEnumerable<TValue> Values
        {
            get
            {
                return _cache.Values;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                return _cache[key];
            }
        }

        public RepositoryCache(IRepository repository)
            : this(repository, new Cache<TKey, TValue>())
        {
        }

        public RepositoryCache(IRepository repository, Cache<TKey, TValue> cache)
            : this(repository, cache, 120000.0)
        {
        }

        public RepositoryCache(IRepository repository, double refreshInterval)
            : this(repository, new Cache<TKey, TValue>(), new Timer(refreshInterval))
        {
        }

        public RepositoryCache(IRepository repository, Cache<TKey, TValue> cache, double refreshInterval)
            : this(repository, new Cache<TKey, TValue>(), new Timer(refreshInterval))
        {
        }

        private RepositoryCache(IRepository repository, Cache<TKey, TValue> cache, Timer timer)
        {
            this._repository = repository;
            this._cache = cache;
            this.timer = timer;
            this.timer.Elapsed += TimerElapsed;
            Refresh();
            this.timer.Start();
        }

        public void Refresh()
        {
            IEnumerable<KeyValuePair<TKey, TValue>> range = QueryFromRepository();
            _cache.Refresh(range);
        }

        public object Add(TKey key, TValue value)
        {
            object result = AddModelFromRepository(value);
            _cache.Add(key, value);
            return result;
        }

        public object Add(TValue value)
        {
            object obj = AddModelFromRepository(value);
            _cache.Add((TKey)obj, value);
            return obj;
        }

        public void Update(TKey key, TValue value)
        {
            UpdateModelFromRepository(value);
            _cache.Update(key, value);
        }

        public TValue Remove(TKey key)
        {
            TValue val = this[key];
            if (val != null)
            {
                DeleteModelFromRepository(val);
                _cache.Remove(key);
            }
            return val;
        }

        protected virtual IEnumerable<KeyValuePair<TKey, TValue>> QueryFromRepository()
        {
            if (_repository == null)
            {
                return null;
            }
            return _repository.Query();
        }

        protected virtual object AddModelFromRepository(TValue value)
        {
            if (_repository == null)
            {
                return null;
            }
            return _repository.Insert(value);
        }

        protected virtual void UpdateModelFromRepository(TValue value)
        {
            if (_repository != null)
            {
                _repository.Modify(value);
            }
        }

        protected virtual void DeleteModelFromRepository(TValue value)
        {
            if (_repository != null)
            {
                _repository.Delete(value);
            }
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Refresh();
        }
    }

}
