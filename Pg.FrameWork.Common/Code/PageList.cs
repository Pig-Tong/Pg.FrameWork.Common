using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pg.FrameWork.Common.Code
{
    /// <summary>
    /// list分页
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageList<T> : List<T>
    {
        /// <summary>
        /// 数据量
        /// </summary>
        public int DataCount { get; set; }

        /// <summary>
        /// 分页数量
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 是否有上一页
        /// </summary>
        public bool HasPreviousPage
        {
            get
            {
                return PageIndex > 1;
            }
        }

        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool HasNextPage
        {
            get
            {
                return PageIndex < PageCount;
            }
        }

        /// <summary>
        /// list分页
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        public PageList(List<T> dataList, int pageSize, int pageIndex)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            DataCount = dataList.Count;
            PageCount = (int)Math.Ceiling((decimal)DataCount / (decimal)pageSize);
            AddRange(dataList.Skip((pageIndex - 1) * pageSize).Take(pageSize));
        }
    }
}
