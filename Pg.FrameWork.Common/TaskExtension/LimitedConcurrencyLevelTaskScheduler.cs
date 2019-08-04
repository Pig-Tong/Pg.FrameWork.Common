using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pg.FrameWork.Common.TaskExtension
{
    public class LimitedConcurrencyLevelTaskScheduler : TaskScheduler
    {
        [ThreadStatic]
        private static bool _currentThreadIsProcessingItems;

        private readonly LinkedList<Task> _tasks = new LinkedList<Task>();

        private readonly int _maxDegreeOfParallelism;

        private int _delegatesQueuedOrRunning;

        public sealed override int MaximumConcurrencyLevel
        {
            get
            {
                return _maxDegreeOfParallelism;
            }
        }

        public LimitedConcurrencyLevelTaskScheduler(int maxDegreeOfParallelism)
        {
            if (maxDegreeOfParallelism < 1)
            {
                throw new ArgumentOutOfRangeException("maxDegreeOfParallelism");
            }
            _maxDegreeOfParallelism = maxDegreeOfParallelism;
        }

        protected sealed override void QueueTask(Task task)
        {
            lock (_tasks)
            {
                _tasks.AddLast(task);
                if (_delegatesQueuedOrRunning < _maxDegreeOfParallelism)
                {
                    _delegatesQueuedOrRunning++;
                    NotifyThreadPoolOfPendingWork();
                }
            }
        }

        private void NotifyThreadPoolOfPendingWork()
        {
            ThreadPool.UnsafeQueueUserWorkItem(delegate
            {
                _currentThreadIsProcessingItems = true;
                try
                {
                    while (true)
                    {
                        Task value;
                        lock (_tasks)
                        {
                            if (_tasks.Count == 0)
                            {
                                _delegatesQueuedOrRunning--;
                                return;
                            }
                            value = _tasks.First.Value;
                            _tasks.RemoveFirst();
                        }
                        TryExecuteTask(value);
                    }
                }
                finally
                {
                    _currentThreadIsProcessingItems = false;
                }
            }, null);
        }

        protected sealed override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            if (!_currentThreadIsProcessingItems)
            {
                return false;
            }
            if (taskWasPreviouslyQueued)
            {
                TryDequeue(task);
            }
            return TryExecuteTask(task);
        }

        protected sealed override bool TryDequeue(Task task)
        {
            lock (_tasks)
            {
                return _tasks.Remove(task);
            }
        }

        protected sealed override IEnumerable<Task> GetScheduledTasks()
        {
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(_tasks, ref lockTaken);
                if (!lockTaken)
                {
                    throw new NotSupportedException();
                }
                return _tasks.ToArray();
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(_tasks);
                }
            }
        }
    }
}
