using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Pg.FrameWork.Common.Code
{
    /// <summary>
    /// 分页返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataContract]
    public class PageResult<T> where T : new()
    {
        /// <summary>
        /// 分页大小
        /// </summary>
        [DataMember]
        public int PageSize { get; set; }

        /// <summary>
        /// 总数据量
        /// </summary>
        [DataMember]
        public int AllCount { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        [DataMember]
        public int PageIndex { get; set; }

        /// <summary>
        /// 分页数据
        /// </summary>
        [DataMember]
        public List<T> Datas { get; set; }
    }
}