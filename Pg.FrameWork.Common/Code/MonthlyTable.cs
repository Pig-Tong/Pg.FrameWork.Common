using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pg.FrameWork.Common.Code
{
    /// <summary>
    /// 按月分表
    /// </summary>
    public class MonthlyTable
    {
        /// <summary>
        /// 获取当前时间表名
        /// </summary>
        /// <returns></returns>
        public static string GetNowTableName()
        {
            return DateTime.Now.ToString("yyyyMM");
        }

        /// <summary>
        /// 根据表名和时间，获取按月分表的表名
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetTableName(string tableName, DateTime date)
        {
            if (date < DateTime.Parse("2019-01-01"))
            {
                return string.Format("{0}{1}", tableName, "201901");
            }
            return string.Format("{0}{1:yyyyMM}", tableName, date);
        }

        /// <summary>
        /// 根据表名，开始时间，结束时间，获取按月分表的表名集合
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static List<string> GetTableName(string tableName, DateTime startDate, DateTime endDate)
        {
            if (endDate.Date > DateTime.Now.Date)
            {
                endDate = DateTime.Now.Date;
            }
            endDate = new DateTime(endDate.Year, endDate.Month, 1).AddMonths(1);
            List<string> list = new List<string>();
            if (startDate.Year == endDate.Year && startDate.Month == endDate.Month)
            {
                list.Add(GetTableName(tableName, startDate));
            }
            else
            {
                DateTime dateTime = startDate.AddDays(-(startDate.Day - 1));
                while (dateTime < endDate)
                {
                    string tableName2 = GetTableName(tableName, dateTime);
                    if (!list.Contains(tableName2))
                    {
                        list.Add(tableName2);
                    }
                    dateTime = dateTime.AddMonths(1);
                }
            }
            return list;
        }
    }
}
