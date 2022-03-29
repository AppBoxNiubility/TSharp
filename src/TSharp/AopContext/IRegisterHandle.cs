#region

using System;

#endregion

namespace TSharp.Core.Message
{
    /// <summary>
    ///     Interface IRegisterHandle
    /// </summary>
    public interface IRegisterHandle : IComparable<IRegisterHandle>
    {
        /// <summary>
        ///     获取消息类型
        ///     <para>by tangjingbo at 2009/11/20 22:57</para>
        /// </summary>
        /// <returns></returns>
        Type MessageType { get; }


        /// <summary>
        ///     获得有效性，默认为true
        ///     <para>by tangjingbo at 2009/11/20 22:58</para>
        /// </summary>
        /// <returns></returns>
        bool IsAlive { get; }

        /// <summary>
        ///     是否监听注册消息类型的子类型，默认为false
        ///     <para>by tangjingbo at 2009/11/20 22:58</para>
        /// </summary>
        /// <returns></returns>
        bool IsListenSubMessage { get; set; }

        /// <summary>
        ///     执行顺序
        ///     <para>by tangjingbo at 2009/12/9 23:52</para>
        /// </summary>
        /// <value></value>
        int Order { get; }

        /// <summary>
        ///     是否监听冒泡消息，默认为true
        ///     <para>by tangjingbo at 2009/11/20 22:59</para>
        /// </summary>
        /// <returns></returns>
        bool IsListenBubble { get; set; }

        /// <summary>
        ///     是否监听广播（向下级发送）消息，默认为true
        ///     <para>by tangjingbo at 2009/11/20 22:59</para>
        /// </summary>
        /// <returns></returns>
        bool IsListenBroadcast { get; set; }

        /// <summary>
        ///     注销该监听器
        /// </summary>
        void UnRegister();

        /// <summary>
        ///     获取注册的消息监听器
        ///     <para>by tangjingbo at 2009/11/20 22:57</para>
        /// </summary>
        /// <returns></returns>
        IHandle GetListener();
    }

    /// <summary>
    ///     消息监听器注册句柄
    ///     <para>by tangjingbo at 2009/11/20 22:54</para>
    /// </summary>
    /// <typeparam name="TMessage">The type of the message.</typeparam>
    public interface IRegisterHandle<TMessage> : IRegisterHandle
    {
        /// <summary>
        ///     获取注册的消息监听器
        ///     <para>by tangjingbo at 2009/11/20 22:57</para>
        /// </summary>
        /// <returns></returns>
        new IHandle<TMessage> GetListener();
    }
}