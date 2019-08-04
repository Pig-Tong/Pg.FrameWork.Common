using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Pg.FrameWork.Common.Code
{
    /// <summary>
    /// 分页请求参数
    /// </summary>
    [DataContract]
    public class Page
    {
        /// <summary>
        /// 页码
        /// </summary>
        [DataMember]
        private int _pageIndex = 1;

        /// <summary>
        /// 分页大小
        /// </summary>
        [DataMember]
        private int _pageSize = 10;

        /// <summary>
        /// 分页大小
        /// </summary>
        [DataMember]
        public int PageIndex
        {
            get
            {
                return _pageIndex;
            }
            set
            {
                _pageIndex = value;
            }
        }

        /// <summary>
        /// 分页大小
        /// </summary>
        [DataMember]
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }

        /// <summary>
        /// 开始行数
        /// </summary>
        public int StratRows
        {
            get
            {
                if (PageIndex <= 0)
                {
                    return 0;
                }
                return PageSize * (PageIndex - 1);
            }
        }

        /// <summary>
        /// 数据查询开始时间
        /// </summary>
        [DataMember]
        public DateTime? StartDate
        {
            get;
            set;
        }

        /// <summary>
        /// 数据查询结束时间
        /// </summary>
        [DataMember]
        public DateTime? EndDate
        {
            get;
            set;
        }
    }
}
