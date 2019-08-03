using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Pg.FrameWork.Common.Code
{
    /// <summary>
    /// 范围
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    [DataContract]
    public struct Range<T>
    {
        /// <summary>
        /// 最小值
        /// </summary>
        private T _lower;

        /// <summary>
        /// 最大值
        /// </summary>
        private T _upper;

        [DataMember]
        public T Lower
        {
            get
            {
                return _lower;
            }
            set
            {
                _lower = value;
            }
        }

        [DataMember]
        public T Upper
        {
            get
            {
                return _upper;
            }
            set
            {
                _upper = value;
            }
        }

        public Range(T lower, T upper)
        {
            this._lower = lower;
            this._upper = upper;
        }
    }
}
