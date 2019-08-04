using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pg.FrameWork.Common.Extension
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEmptySpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this ICollection list)
        {
            if (list != null)
            {
                return list.Count <= 0;
            }
            return false;
        }

        /// <summary>
        /// 是否为success
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsSuccess(this string value)
        {
            return value.ToLower() == "success";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UnicodeToChineseWord(this string str)
        {
            Regex regex = new Regex("(?i)\\\\u([0-9a-f]{4})");
            return regex.Replace(str, (Match m1) => ((char)(ushort)Convert.ToInt32(m1.Groups[1].Value, 16)).ToString());
        }

        /// <summary>
        /// 去掉html标签
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveHtml(this string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return "";
            }
            string pattern = "<style[^>]*?>[\\s\\S]*?<\\/style>";
            string pattern2 = "<script[^>]*?>[\\s\\S]*?<\\/script>";
            string pattern3 = "<[^>]+>";
            content = Regex.Replace(content, pattern, "");
            content = Regex.Replace(content, pattern2, "");
            content = Regex.Replace(content, pattern3, "");
            content = Regex.Replace(content, "\\s*|\t|\r|\n", "");
            content = content.Replace(" ", "");
            content = content.Replace("\"", "");
            content = content.Replace("\"", "");
            return content.Trim();
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static string[] Split(this string source, string separator)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (separator == null)
            {
                throw new ArgumentNullException("separator");
            }
            string[] array = new string[1];
            int num = source.IndexOf(separator, 0, StringComparison.Ordinal);
            if (num < 0)
            {
                array[0] = source;
                return array;
            }
            array[0] = source.Substring(0, num);
            return Split(source.Substring(num + separator.Length), separator, array);
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="source"></param>
        /// <param name="separator"></param>
        /// <param name="attachArray"></param>
        /// <returns></returns>
        private static string[] Split(string source, string separator, string[] attachArray)
        {
            string[] array;
            while (true)
            {
                array = new string[attachArray.Length + 1];
                attachArray.CopyTo(array, 0);
                int num = source.IndexOf(separator, 0, StringComparison.Ordinal);
                if (num < 0)
                {
                    break;
                }
                array[attachArray.Length] = source.Substring(0, num);
                source = source.Substring(num + separator.Length);
                attachArray = array;
            }
            array[attachArray.Length] = source;
            return array;
        }

        /// <summary>
        /// 字符串转十六进制
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StrToHex(this string str)
        {
            string text = "";
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            byte[] bytes = Encoding.Default.GetBytes(str);
            for (int i = 0; i < bytes.Length; i++)
            {
                text += bytes[i].ToString("X");
            }
            return text;
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
