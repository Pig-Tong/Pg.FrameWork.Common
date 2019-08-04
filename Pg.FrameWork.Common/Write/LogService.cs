using System;
using System.IO;
using System.Text;

namespace Pg.FrameWork.Common.Write
{
    /// <summary>
    /// 日志服务
    /// </summary>
    public static class LogService
    {
        /// <summary>
        /// 锁关键字
        /// </summary>
        private static readonly object Obj = new object();

        /// <summary>
        /// 记录日常日志
        /// </summary>
        /// <param name="ex"></param>
        public static void WriteLog(System.Exception ex)
        {
            WriteLog(ex, null);
        }

        /// <summary>
        /// 记录日常日志，带路径
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="path"></param>
        public static void WriteLog(System.Exception ex, string path)
        {
            StringBuilder stringBuilder = CreateErrorMessage(ex);
            WriteLog(stringBuilder.ToString(), path ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs/ExceptionLog"));
        }

        /// <summary>
        /// 记录日常日志，带描述，路径
        /// </summary>
        /// <param name="describe"></param>
        /// <param name="ex"></param>
        /// <param name="path"></param>
        public static void WriteLog(string describe, System.Exception ex, string path)
        {
            StringBuilder arg = CreateErrorMessage(ex);
            WriteLog(string.Format("Describe:{0} Error:{1}", describe, arg), path ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs/ExceptionLog"));
        }

        /// <summary>
        /// 创建错误异常日志内容
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private static StringBuilder CreateErrorMessage(System.Exception ex)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("异常消息：" + ex.Message);
            stringBuilder.AppendLine("堆栈信息：" + ex.StackTrace);
            stringBuilder.AppendLine("异常方法：" + ex.TargetSite.Name);
            if (ex.InnerException != null)
            {
                stringBuilder.AppendLine("异常消息：" + ex.InnerException.Message);
                stringBuilder.AppendLine("堆栈信息：" + ex.InnerException.StackTrace);
                stringBuilder.AppendLine("异常方法：" + ex.InnerException.TargetSite.Name);
                if (ex.InnerException.InnerException != null)
                {
                    stringBuilder.AppendLine("异常消息：" + ex.InnerException.InnerException.Message);
                    stringBuilder.AppendLine("堆栈信息：" + ex.InnerException.InnerException.StackTrace);
                    stringBuilder.AppendLine("异常方法：" + ex.InnerException.InnerException.TargetSite.Name);
                }
            }
            return stringBuilder;
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="content"></param>
        public static void WriteLog(string content)
        {
            WriteLog(content, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs"));
        }

        /// <summary>
        /// 记录日志，带路径
        /// </summary>
        /// <param name="content"></param>
        /// <param name="path"></param>
        public static void WriteLog(string content, string path)
        {
            Action action = delegate
            {
                Log(content, path);
            };
            action.BeginInvoke(null, null);
        }

        /// <summary>
        /// 记录日志，带路径
        /// </summary>
        /// <param name="content"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static bool Log(string content, string path)
        {
            lock (Obj)
            {
                try
                {
                    TextWriter textWriter = new TextWriter(path);
                    return textWriter.WriteLog(DateTime.Now.ToString("日志时间:yyyy-MM-dd HH:mm:ss") + Environment.NewLine + content + Environment.NewLine);
                }
                catch (System.Exception)
                {
                    return false;
                }
            }
        }
    }
}
