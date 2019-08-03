using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pg.FrameWork.Common.Write
{
    public static class LogService
    {
        private static readonly object Obj = new object();

        public static void WriteLog(Exception ex)
        {
            WriteLog(ex, null);
        }

        public static void WriteLog(Exception ex, string path)
        {
            StringBuilder stringBuilder = CreateErrorMessage(ex);
            WriteLog(stringBuilder.ToString(), path ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs/ExceptionLog"));
        }

        public static void WriteLog(string describe, Exception ex, string path)
        {
            StringBuilder arg = CreateErrorMessage(ex);
            WriteLog(string.Format("Describe:{0} Error:{1}", describe, arg), path ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs/ExceptionLog"));
        }

        private static StringBuilder CreateErrorMessage(Exception ex)
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

        public static void WriteLog(string content)
        {
            WriteLog(content, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs"));
        }

        public static void WriteLog(string content, string path)
        {
            Action action = delegate
            {
                Log(content, path);
            };
            action.BeginInvoke(null, null);
        }

        internal static bool Log(string content, string path)
        {
            lock (Obj)
            {
                try
                {
                    TextWriter textWriter = new TextWriter(path);
                    return textWriter.WriteLog(DateTime.Now.ToString("日志时间:yyyy-MM-dd HH:mm:ss") + Environment.NewLine + content + Environment.NewLine);
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
