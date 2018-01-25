using System;

namespace Hydra.Admin.EventBus.Mq
{
    public interface IMessageQueue
    {
        /// <summary>
        /// 向消息队列中发送消息
        /// </summary>
        void Send(string messageId, object message);

        /// <summary>
        /// 从消息队列中轮询消息
        /// </summary>
        void Receive<T>(Action<string,T> action);
    }
}
