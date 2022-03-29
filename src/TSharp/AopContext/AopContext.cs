using System;
using System.Linq.Expressions; 
using TSharp.Core.Osgi;
 
namespace TSharp.Core
{
  using TSharp.Core.Message;
  using TSharp.Core.Pattern;
  using TSharp.Core.Util;

  /// <summary>
    /// AOP容器上下文
    /// <para>by tangjingbo at 2009-11-4 14:37</para>
    /// </summary>
    public static partial class AopContext
    {
        private static IServiceSituation _serviceSituation;

        #region 获取和重新创建上下文

        private static readonly object AsynSituation = new object();

        /// <summary>
        /// aop上下文
        /// </summary>
        public static IServiceSituation Services
        {
            get
            {
                if (_serviceSituation != null)
                    return _serviceSituation;
                lock (AsynSituation)
                {
                    if (_serviceSituation == null)
                        _serviceSituation = CreateServiceSituation();
                    return _serviceSituation;
                }
            }
        }

        /// <summary>
        /// 重新创建服务访问上下文，返回原有上下文
        /// </summary>
        /// <returns></returns>
        public static IServiceSituation ResetServiceSituation()
        {
            lock (AsynSituation)
            {
                IServiceSituation origin = _serviceSituation;
                _serviceSituation = CreateServiceSituation();
                return origin;
            }
        }

        #endregion

        /// <summary>
        /// 获取默认服务定位器
        /// </summary>
        /// <value>The default.</value>
        public static IServiceLocator GetDefaultLactor()
        {
            return Services.GetRequest();
        }

        /// <summary>
        /// Gets the HTTP context.
        /// </summary>
        /// <returns></returns>
        public static IContext GetContext()
        {
            return _contextFactory();
        }

        

        /// <summary>
        /// Sets the HTTP context factory.
        /// </summary>
        /// <param name="fac">The fac.</param>
        public static void SetHttpContextFactory(Expression<Func<IContext>> fac)
        {
            _contextFactory = fac.Compile();
        }
        internal static readonly string KeyRequestReadWriteUnitOfWork = "Key:Request:ReadWriteUnitOfWork".GetAppSetting("Key:Request:ReadWriteUnitOfWork");

      
     

        /// <summary>
        /// Gets the situation.
        /// </summary>
        /// <returns>ISituation.</returns>
        public static ISituationFactory GetSituationFactory()
        {
            return ThreadSingletonHelper<ISituationFactory>.GetOrAdd(LazyLoading.New<ISituationFactory>);
        }

        /// <summary>
        /// Registers the specified handle.
        /// </summary>
        /// <typeparam name="TMessage">The type of the T message.</typeparam>
        /// <param name="handle">The handle.</param>
        /// <param name="order">The order.</param>
        /// <returns>IRegisterHandle{``0}.</returns>
        public static IRegisterHandle<TMessage> Register<TMessage>(IHandle<TMessage> handle, int order)
        {
            return GetSituationFactory().GetSituation().Register(handle, order);
        }

        /// <summary>
        /// Registers the specified handle.
        /// </summary>
        /// <typeparam name="TMessage">The type of the T message.</typeparam>
        /// <param name="handle">The handle.</param>
        /// <returns>IRegisterHandle{``0}.</returns>
        public static IRegisterHandle<TMessage> Register<TMessage>(IHandle<TMessage> handle)
        {
            return GetSituationFactory().GetSituation().Register(handle);
        }

        /// <summary>
        /// Registers the specified handle.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <returns>IRegisterHandle[][].</returns>
        public static IRegisterHandle[] Register(IHandle handle)
        {
            return GetSituationFactory().GetSituation().Register(handle);
        }
        /// <summary>
        /// 广播一个消息
        /// </summary>
        /// <typeparam name="TMessage">The type of the T message.</typeparam>
        /// <param name="message">The message.</param>
        /// <param name="order">The order.</param>
        /// <param name="maxDistance">The max distance.</param>
        /// <returns>IRegisterHandle{``0}.</returns>
        public static IMessageResult<TMessage> Broadcast<TMessage>(TMessage message, int order, int maxDistance)
        {
            return GetSituationFactory().GetSituation().Broadcast(message, maxDistance);
        }
        /// <summary>
        /// Broadcasts the specified message.
        /// </summary>
        /// <typeparam name="TMessage">The type of the T message.</typeparam>
        /// <param name="message">The message.</param>
        /// <returns>IMessageResult{``0}.</returns>
        public static IMessageResult<TMessage> Broadcast<TMessage>(TMessage message)
        {
            return GetSituationFactory().GetSituation().Broadcast(message);
        }
    }
}