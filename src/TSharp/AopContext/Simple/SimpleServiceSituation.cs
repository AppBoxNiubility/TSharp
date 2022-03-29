using System;
using System.Collections.Generic;
using Common.Logging;
using TSharp.Core.Osgi;
using TSharp.Core.Simple;
using TSharp.Core;
[assembly: RegLazyLoading(typeof(IServiceSituation), typeof(SimpleServiceSituation), Priority = LoadingPriority.__InnerLowest)]
namespace TSharp.Core.Simple
{
    /// <summary>
    /// 服务定位情景上下文实现
    /// </summary>
    /// <remarks>
    /// 1、2011-09-28 需要进行重构以便优化代码 @tangjingbo
    /// </remarks>
    /// <author>
    /// tangjingbo
    /// </author>
    public partial class SimpleServiceSituation : IServiceSituation, IDisposable
    {
        private readonly ILog log = LogManager.GetLogger("SimpleServiceSituation");
        /// <summary>
        /// 
        /// </summary>
        public bool ThrowExceptionNullSessionRequest;

        #region 常量

        /// <summary>
        /// 系统容器key
        /// </summary>
        private const string CACHEKEY_SYSTEM_CONTAINER = "CONTAINER_SYSTEM";

        /// <summary>
        /// 会话容器KEY
        /// </summary>
        private const string CACHEKEY_SESSION_CONTAINER = "CONTAINER_SESSION";

        /// <summary>
        /// 请求容器key
        /// </summary>
        private const string CACHEKEY_REQUSET_CONTAINER = "CONTAINER_REQUEST";

        /// <summary>
        /// 线程容器Key
        /// </summary>
        private const string CACHEKEY_THREAD_CONTAINER = "CONTAINER_THREAD";


        /// <summary>
        /// 系统级别容器名称
        /// </summary>
        public static string CONFIG_CONTAINER_NAME_SYSTEM = "system";

        /// <summary>
        /// 会话级别容器名称
        /// </summary>
        public static string CONFIG_CONTAINER_NAME_SESSION = "session";

        /// <summary>
        /// 请求级别容器名称
        /// </summary>
        public static string CONFIG_CONTAINER_NAME_REQUEST = "request";

        /// <summary>
        /// 请求级别容器名称
        /// </summary>
        public static string CONFIG_CONTAINER_NAME_THREAD = "thread";

        #endregion

        #region IDisposable Members

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
#if !WebSvrLocator
            Dispose(_root);
            _root = null;
#endif
        }

        #endregion

        #region IServiceSituation Members

        /// <summary>
        /// 请求级别服务定位器。winform首先Session子容器，然后返回；web中每次请求有不同实例，请求内共享。
        /// </summary>
        /// <returns></returns>
        public IServiceLocator GetRequest()
        {
            return new SimpleLocatorWrapper(Level.REQUSET);
        }

        /// <summary>
        /// 会话级别服务定位器。先找会话服务，然后找系统服务
        /// </summary>
        /// <returns></returns>
        public IServiceLocator GetSession()
        {
            return new SimpleLocatorWrapper(Level.SESSION);
        }

        /// <summary>
        /// 线程级别服务定位器。查找线程级服务，然后再查找系统服务
        /// </summary>
        /// <returns></returns>
        public IServiceLocator GetThread()
        {
            return new SimpleLocatorWrapper(Level.THREAD);
        }

        /// <summary>
        /// 系统级别服务定位器.web程序存储在HttpAppliction中，应用程序域内多线程共享
        /// <para>by tangjingbo at 2009-11-4 14:37</para>
        /// </summary>
        /// <returns></returns>
        public IServiceLocator GetRoot()
        {
            return new SimpleLocatorWrapper(Level.ROOT);
        }

        #endregion

        #region Helper



        private void Configure(IEnumerable<RegServiceAttribute> services, SimpleLocatorWrapper unityContainer)
        {
            foreach (RegServiceAttribute lb in services)
            {
                Register(lb, unityContainer);
            }
        }

        private void Register(RegServiceAttribute lb, SimpleLocatorWrapper unityContainer)
        {

        }

        #endregion

        private void Dispose(IDisposable dispose)
        {
            if (dispose != null)
                dispose.Dispose();
        }



    }

    #region Nested type: ServiceLocatorException

    #endregion

}