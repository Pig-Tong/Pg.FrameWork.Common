using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pg.FrameWork.Common.Extension
{
    /// <summary>
    /// 时间扩展
    /// </summary>
    public static class DateExtension
    {
        /// <summary>
        /// 转YYYYMMdd
        /// </summary>
        /// <param name="date"></param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static string ToYYYYMMdd(this DateTime date, string separator = "-")
        {
            return date.ToString(string.Format("yyyy{0}MM{0}dd", separator));
        }

        /// <summary>
        /// 转YYYYMMddHH
        /// </summary>
        /// <param name="date"></param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static string ToYYYYMMddHH(this DateTime date, string separator = "-")
        {
            return date.ToString(string.Format("yyyy{0}MM{0}dd HH", separator));
        }

        /// <summary>
        /// 转YYYYMMddHHmm
        /// </summary>
        /// <param name="date"></param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static string ToYYYYMMddHHmm(this DateTime date, string separator = "-")
        {
            return date.ToString(string.Format("yyyy{0}MM{0}dd HH:mm", separator));
        }

        /// <summary>
        /// 转YYYYMMddHHmmss
        /// </summary>
        /// <param name="date"></param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static string ToYYYYMMddHHmmss(this DateTime date, string separator = "-")
        {
            return date.ToString(string.Format("yyyy{0}MM{0}dd HH:mm:ss", separator));
        }

        /// <summary>
        /// 获取时间戳(秒级，毫秒级要*1000)
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp(this DateTime date)
        {
            TimeSpan ts = date - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }
    }
}
