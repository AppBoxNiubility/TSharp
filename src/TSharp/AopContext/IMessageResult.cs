namespace System
{
    /// <summary>
    ///     消息结果
    ///     <para>by tangjingbo at 2009-11-20 16:08</para>
    /// </summary>
    /// <typeparam name="TMessage">The type of the message.</typeparam>
    public interface IMessageResult<TMessage>
    {
        /// <summary>
        ///     获取该消息此次传输传递到当前监听器前已经被监听（处理）的次数
        /// </summary>
        int GetListeneds { get; }

        /// <summary>
        ///     是否被中止
        /// </summary>
        bool IsTerminated { get; }

        /// <summary>
        ///     获得当前消息对象
        /// </summary>
        TMessage GetMessage();
    }
}