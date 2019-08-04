using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Pg.FrameWork.Common.Code
{
    /// <summary>
    /// 公用返回结果，带data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataContract]
    public class CommonResult<T> : Result
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        [DataMember]
        public T Data { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CommonResult()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="success">是否成功</param>
        public CommonResult(bool success)
            : base(success)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="success">是否成功</param>
        /// <param name="message">返回消息</param>
        public CommonResult(bool success, string message)
            : base(success)
        {
            base.Message = message;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="success">是否成功</param>
        /// <param name="message">返回消息</param>
        /// <param name="data">数据</param>
        public CommonResult(bool success, string message, T data)
            : this(success, message)
        {
            Data = data;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">返回消息</param>
        public CommonResult(string message)
            : this(false, message)
        {
            Data = default(T);
        }
    }
}
