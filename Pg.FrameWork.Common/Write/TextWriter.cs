using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pg.FrameWork.Common.Write
{
    internal class TextWriter
    {
        private readonly string _fileName;

        public TextWriter()
            : this(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs"))
        {
        }

        public TextWriter(string fileName)
        {
            this._fileName = fileName;
        }

        internal bool WriteLog(string logInfo)
        {
            if (string.IsNullOrWhiteSpace(logInfo))
            {
                return false;
            }
            DateTime now = DateTime.Now;
            string fileMainPath = GetFileMainPath(now);
            FileInfo lastAccessFile = GetLastAccessFile(fileMainPath, now);
            FileStream fileStream = GetFileStream(lastAccessFile, fileMainPath, now);
            if (fileStream == null)
            {
                return false;
            }
            try
            {
                StreamWriter streamWriter = new StreamWriter(fileStream);
                streamWriter.BaseStream.Seek(0L, SeekOrigin.End);
                streamWriter.Write(logInfo);
                streamWriter.Flush();
                streamWriter.Close();
                return true;
            }
            finally
            {
                fileStream.Close();
                fileStream.Dispose();
            }
        }

        private string GetFileMainPath(DateTime timeStamp)
        {
            return Path.Combine(_fileName, timeStamp.ToString("yyyyMMdd"));
        }

        private static FileInfo GetLastAccessFile(string path, DateTime timeStamp)
        {
            FileInfo result = null;
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (directoryInfo.Exists)
            {
                FileInfo[] files = directoryInfo.GetFiles();
                FileInfo[] array = files;
                foreach (FileInfo fileInfo in array)
                {
                    if (timeStamp.Hour == fileInfo.CreationTime.Hour)
                    {
                        result = fileInfo;
                        break;
                    }
                }
            }
            else
            {
                directoryInfo.Create();
            }
            return result;
        }

        private static FileStream GetFileStream(FileInfo fileInfo, string path, DateTime timeStamp)
        {
            if (fileInfo == null)
            {
                try
                {
                    return CreateFile(path, GetFileMainName(timeStamp));
                }
                catch (Exception)
                {
                    return null;
                }
            }
            if (IsOutOfTimeMaxLength(fileInfo.CreationTime, timeStamp))
            {
                return CreateFile(path, GetFileMainName(timeStamp));
            }
            try
            {
                return fileInfo.OpenWrite();
            }
            catch
            {
                return CreateFile(path, GetFileMainName(timeStamp));
            }
        }

        private static FileStream CreateFile(string path, string fileName1)
        {
            return File.Create(string.Format("{0}\\{1}.log", path, fileName1));
        }

        private static string GetFileMainName(DateTime timeStamp)
        {
            return timeStamp.ToString("HH");
        }

        private static bool IsOutOfTimeMaxLength(DateTime creationTime, DateTime timeStamp)
        {
            return Math.Abs((creationTime - timeStamp).TotalHours) >= 1.0;
        }
    }
}
