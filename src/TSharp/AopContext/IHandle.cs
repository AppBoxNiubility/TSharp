#region

using System;

#endregion

namespace TSharp.Core.Message
{
    /// <summary>
    ///     消息监听器,这是一个标记接口
    /// </summary>
    public interface IHandle
    {
        /// <summary>
        /// Occurs when [disposed].
        /// </summary>
        event EventHandler Disposed;
    }

    /// <summary>
    ///     消息监听器
    /// </summary>
    /// <typeparam name="TMessage">消息类型</typeparam>
    public interface IHandle<TMessage> : IHandle
    {
        /// <summary>
        ///     Handles the specified sender.
        /// </summary>
        /// <param name="msg">消息参数</param>
        /// <param name="transmitter">消息控制器</param>
        void OnReceive(TMessage msg,
                       ITransmitter<TMessage> transmitter);

    }
}