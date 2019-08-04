using System;
using System.IO;
using System.Threading;
using Pg.FrameWork.Common.Write;

namespace Pg.FrameWork.Common.Cache
{
    public abstract class ICacheBase
    {
        public enum RunningStatus
        {
            未开始,
            正常运行,
            停止
        }

        private readonly string _defaultLogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        private Thread _thread;

        private RunningStatus _runningStatus;

        protected abstract int IncrementFrequency
        {
            get;
        }

        protected abstract int FullFrequency
        {
            get;
        }

        protected abstract string ProgramName
        {
            get;
        }

        protected abstract DateTime CuurentDateTime
        {
            get;
        }

        protected virtual string LogPath
        {
            get
            {
                return null;
            }
        }

        protected DateTime LastGetTime
        {
            get;
            set;
        }

        protected DateTime LastALlTime
        {
            get;
            set;
        }

        protected DateTime LastDbUpdateTime
        {
            get;
            set;
        }

        public event Action<int> OnFullDoesEven;

        protected ICacheBase()
        {
            _runningStatus = RunningStatus.未开始;
        }

        public void Start()
        {
            _thread = new Thread(StartMethod)
            {
                IsBackground = true,
                Name = "XX：XX缓存线程"
            };
            _thread.Start();
            _runningStatus = RunningStatus.正常运行;
        }

        public void Stop()
        {
            try
            {
                _thread.Abort();
                _thread = null;
            }
            finally
            {
                _runningStatus = RunningStatus.停止;
            }
        }

        private void StartMethod()
        {
            try
            {
                int num = FullDoes();
                Action<int> onFullDoesEven = this.OnFullDoesEven;
                if (onFullDoesEven != null)
                {
                    onFullDoesEven(num);
                }
                LogService.WriteLog(string.Format("{0}全量更新完成,影响条数:{1}", ProgramName, num), GetLogPath());
                while (_runningStatus == RunningStatus.正常运行)
                {
                    DateTime now = DateTime.Now;
                    double totalMilliseconds = (now - LastGetTime).TotalMilliseconds;
                    if (IncrementFrequency > 0 && (totalMilliseconds >= (double)IncrementFrequency || LastGetTime == DateTime.MinValue))
                    {
                        try
                        {
                            LastDbUpdateTime = ((LastDbUpdateTime == DateTime.MinValue) ? CuurentDateTime : LastDbUpdateTime);
                            int num2 = Increment();
                            LastGetTime = now;
                            LastDbUpdateTime = CuurentDateTime;
                            LogService.WriteLog(string.Format("{0}增量更新完成,影响条数:{1}", ProgramName, num2), GetLogPath());
                        }
                        catch (System.Exception ex)
                        {
                            LogService.WriteLog(ex, GetLogPath());
                        }
                    }
                    Thread.Sleep(3000);
                    LogService.WriteLog(string.Format("{0}缓存程序正常运行", ProgramName), GetLogPath() + "/Run");
                }
            }
            catch (System.Exception ex2)
            {
                LogService.WriteLog("运行异常", ex2, GetLogPath());
            }
        }

        protected abstract int Increment();

        protected abstract int FullDoes();

        private string GetLogPath()
        {
            if (LogPath == null)
            {
                return _defaultLogPath;
            }
            return Path.Combine(_defaultLogPath, LogPath);
        }
    }
}
