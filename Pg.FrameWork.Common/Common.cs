using System;
using System.Collections;
using System.Collections.Generic;
using Pg.FrameWork.Common.Code;

namespace Pg.FrameWork.Common
{
    /// <summary>
    /// 公共方法
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// Dictionary转HashTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        /// <param name="sPara"></param>
        /// <returns></returns>
        public static Hashtable ToHashTable<T, TH>(this SortedDictionary<T, TH> sPara)
        {
            Hashtable hashtable = new Hashtable();
            foreach (KeyValuePair<T, TH> item in sPara)
            {
                hashtable.Add(item.Key, item.Value);
            }
            return hashtable;
        }

        /// <summary>
        /// 根据时间范围，获取表名集合
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public static List<string> GetTableNames(Range<DateTime> range)
        {
            DateTime dateTime = new DateTime(2014, 5, 1);
            DateTime today = DateTime.Today;
            if (range.Lower < dateTime)
            {
                range.Lower = dateTime;
            }
            if (range.Upper > today)
            {
                range.Upper = today;
            }
            List<string> list = new List<string>();
            int num = range.Upper.Year * 12 + range.Upper.Month - (range.Lower.Year * 12 + range.Lower.Month);
            for (int i = 0; i <= num; i++)
            {
                list.Add(range.Lower.AddMonths(i).ToString("yyyyMM"));
            }
            return list;
        }

        /// <summary>
        /// 获取年份和月份（yyyyMM）
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetYearMonth(DateTime dt)
        {
            return dt.ToString("yyyyMM");
        }

        /// <summary>
        /// 获取年份（yyyy）
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetYear(DateTime dt)
        {
            return dt.ToString("yyyy");
        }

        /// <summary>
        /// 生成Guid
        /// </summary>
        /// <returns></returns>
        public static string BuildGuid()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
   }
}
