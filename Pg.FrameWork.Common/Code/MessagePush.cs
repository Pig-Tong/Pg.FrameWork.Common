using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pg.FrameWork.Common.Code
{
    /// <summary>
    /// 委托
    /// </summary>
    /// <param name="message"></param>
    public delegate void MessageChangedHandler(string message);

    /// <summary>
    /// 消息推送
    /// </summary>
    public class MessagePush
    {
        public static event MessageChangedHandler MessageChanged;

        public static void SendMessage(string message)
        {
            if (MessagePush.MessageChanged != null)
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    try
                    {
                        MessagePush.MessageChanged(message);
                    }
                    catch
                    {
                        // ignored
                    }
                });
            }
        }
    }
}
