using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Pg.FrameWork.Common
{
    public class WebHelper
    {
        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string HttpGet(string url)
        {
            HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
            if (httpWebRequest != null)
            {
                httpWebRequest.Method = "GET";
                httpWebRequest.ContentType = "text/html;charset=UTF-8";
                httpWebRequest.Timeout = int.MaxValue;
                HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
                StreamReader streamReader = null;
                try
                {
                    streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                    return streamReader.ReadToEnd();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (httpWebResponse != null)
                    {
                        var responseStream = httpWebResponse.GetResponseStream();
                        if (responseStream != null) responseStream.Close();
                    }
                    if (streamReader != null) streamReader.Close();
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postdata"></param>
        /// <returns></returns>
        public static string HttpPost(string url, string postdata)
        {
            HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
            if (httpWebRequest != null)
            {
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Timeout = int.MaxValue;
                byte[] bytes = Encoding.UTF8.GetBytes(postdata);
                using (Stream stream = httpWebRequest.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
                StreamReader streamReader = null;
                try
                {

                    streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                    char[] array = new char[256];
                    StringBuilder stringBuilder = new StringBuilder();
                    int charCount;
                    while ((charCount = streamReader.Read(array, 0, array.Length)) > 0)
                    {
                        stringBuilder.Append(array, 0, charCount);
                    }
                    return stringBuilder.ToString();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    httpWebResponse.GetResponseStream().Close();
                    streamReader.Close();
                }
            }
            return string.Empty;
        }
    }
}
