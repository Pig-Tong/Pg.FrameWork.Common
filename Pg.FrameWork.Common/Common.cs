using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Pg.FrameWork.Common.Code;

namespace Pg.FrameWork.Common
{
    /// <summary>
    /// 公共方法
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// 验证订单号
        /// </summary>
        /// <param name="ticketOrderNumber"></param>
        /// <returns></returns>
        public static bool VerifyTicketOrderNumber(this string ticketOrderNumber)
        {
            if (string.IsNullOrWhiteSpace(ticketOrderNumber))
            {
                return false;
            }
            if (ticketOrderNumber.Length < 6)
            {
                return false;
            }
            DateTime result;
            return DateTime.TryParse("20" + ticketOrderNumber.Substring(0, 2) + "-" + ticketOrderNumber.Substring(2, 2), out result);
        }

        /// <summary>
        /// 获取枚举的描述信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            string text = value.ToString();
            Type type = value.GetType();
            FieldInfo field = type.GetField(text);
            object[] customAttributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (customAttributes.Length == 0)
            {
                return text;
            }
            DescriptionAttribute descriptionAttribute = (DescriptionAttribute)customAttributes[0];
            return descriptionAttribute.Description;
        }

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
        /// 格式化金钱
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public static string ConvertPoints(this decimal price)
        {
            return (price * 100m).ToString("0.##");
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

        /// <summary>
        /// 获取MD5加密字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetMd5Hash(this string input, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] array = mD5CryptoServiceProvider.ComputeHash(encoding.GetBytes(input));
            StringBuilder stringBuilder = new StringBuilder();
            byte[] array2 = array;
            foreach (byte b in array2)
            {
                stringBuilder.Append(b.ToString("x2"));
            }
            return stringBuilder.ToString();
        }
    }
}
