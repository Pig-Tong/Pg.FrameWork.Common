using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pg.FrameWork.Common.Extension
{
    /// <summary>
    /// url扩展
    /// </summary>
    public static class UrlExtension
    {
        /// <summary>
        /// Url编码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlEncode(this string url)
        {
            return Uri.EscapeDataString(Uri.EscapeUriString(url));
            //return HttpUtility.UrlEncode(url, Encoding.UTF8);
        }

        /// <summary>
        /// Url解码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlDecode(this string url)
        {
            return Uri.UnescapeDataString(url);
            //return HttpUtility.UrlDecode(url, Encoding.UTF8);
        }
    }
}
