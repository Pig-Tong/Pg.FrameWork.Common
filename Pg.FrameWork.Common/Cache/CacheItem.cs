using System;

namespace Pg.FrameWork.Common.Cache
{
    internal class CacheItem<T>
    {
        private T _mvalue;

        public DateTime Time
        {
            get;
            private set;
        }

        public T Value
        {
            get
            {
                return _mvalue;
            }
            set
            {
                _mvalue = value;
                Time = DateTime.Now;
            }
        }

        public CacheItem(T value)
        {
            Time = DateTime.Now;
            _mvalue = value;
        }
    }
}
