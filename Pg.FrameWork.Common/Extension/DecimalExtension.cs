using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pg.FrameWork.Common.Extension
{
    /// <summary>
    /// 金钱扩展
    /// </summary>
    public static class DecimalExtension
    {
        /// <summary>
        /// 获取小数点左边数字长度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int DecimalLeftLength(this decimal value)
        {
            string text = Convert.ToString(value, CultureInfo.InvariantCulture);
            int num = text.IndexOf('.');
            if (num < 0)
            {
                return 0;
            }
            return text.Substring(text.IndexOf('.') + 1).Length;
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
    }
}
