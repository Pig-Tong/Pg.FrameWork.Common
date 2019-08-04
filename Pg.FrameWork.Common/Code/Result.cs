using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Pg.FrameWork.Common.Code
{
    /// <summary>
    /// 结果返回参数
    /// </summary>
    [DataContract]
    public class Result
    {
        /// <summary>
        /// 状态码
        /// </summary>
        private int _code;

        /// <summary>
        /// 是否成功
        /// </summary>
        [DataMember]
        public bool IsSucceed { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// 状态码
        /// </summary>
        [DataMember]
        public int Code
        {
            get
            {
                if (_code != 0)
                {
                    return _code;
                }
                if (!IsSucceed)
                {
                    return 403;
                }
                return 200;
            }
            set
            {
                _code = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Result(bool isSucceed)
        {
            IsSucceed = isSucceed;
            Message = string.Empty;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Result()
        {
            IsSucceed = false;
            Message = string.Empty;
        }
    }
}
