using System;
using System.Linq;
using TSharp.Core.Pattern;
using Common.Logging;
using TSharp.Core;
namespace TSharp.Core
{
    /// <summary>
    /// Class WebContext
    /// </summary>
    public class WebContext : IContext
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="WebContext"/> class from being created.
        /// </summary>
        private WebContext()
        {

        }
        /// <summary>
        /// The _instance
        /// </summary>
        private static IContext _instance = new WebContext();
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static IContext Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// Class ApplicationImpl
        /// </summary>
        class ApplicationImpl : IApplicationState
        {
            /// <summary>
            /// The instance
            /// </summary>
            static internal ApplicationImpl Instance = new ApplicationImpl();
            /// <summary>
            /// Prevents a default instance of the <see cref="ApplicationImpl"/> class from being created.
            /// </summary>
            private ApplicationImpl()
            {

            }

            /// <summary>
            /// Gets or sets the <see cref="System.Object"/> with the specified key.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>System.Object.</returns>
            public object this[string key]
            {
                get
                {
                    return HttpContextHelper.Current.Application[key];
                }
                set
                {
                    try
                    {
                        HttpContextHelper.Current.Application[key] = value;
                    }
                    finally
                    {
                    }

                }
            }


            /// <summary>
            /// Removes the specified key.
            /// </summary>
            /// <param name="key">The key.</param>
            public void Remove(string key)
            {
                try
                {
                    HttpContextHelper.Current.Application.Lock();
                    HttpContextHelper.Current.Application.Remove(key);
                }
                finally
                {
                    HttpContextHelper.Current.Application.UnLock();
                }
            }

            /// <summary>
            /// Gets the sync root.
            /// </summary>
            /// <value>The sync root.</value>
            /// <exception cref="System.NotImplementedException"></exception>
            /// <exception cref="NotImplementedException"></exception>
            public object SyncRoot
            {
                get { throw new NotImplementedException(); }
            }


            public object GetOrAdd(string key, Func<string, object> fac)
            {
                if (HttpContextHelper.Current.Application.AllKeys.Contains(key))
                    return this[key];
                try
                {
                    HttpContextHelper.Current.Application.Lock();
                    if (HttpContextHelper.Current.Application.AllKeys.Contains(key))
                        return this[key];
                    this[key] = fac(key);
                }
                finally
                {
                    HttpContextHelper.Current.Application.UnLock();
                }
                return this[key];
            }
        }




        /// <summary>
        /// Gets the application.
        /// </summary>
        /// <value>The application.</value>
        public IApplicationState Application
        {
            get { return ApplicationImpl.Instance; }
        }
        /// <summary>
        /// Class SessionImpl
        /// </summary>
        class SessionImpl : ISessionState
        {
            /// <summary>
            /// Gets or sets the <see cref="System.Object"/> with the specified key.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>System.Object.</returns>
            public object this[string key]
            {
                get
                {
                    return HttpContextHelper.Current.Session[key];
                }
                set
                {
                    try
                    {

                        HttpContextHelper.Current.Session[key] = value;
                    }
                    finally
                    {
                    }

                }
            }


            /// <summary>
            /// Removes the specified key.
            /// </summary>
            /// <param name="key">The key.</param>
            public void Remove(string key)
            {
                HttpContextHelper.Current.Session.Remove(key);
            }

            /// <summary>
            /// Gets the sync root.
            /// </summary>
            /// <value>The sync root.</value>
            public object SyncRoot
            {
                get { return HttpContextHelper.Current.Session.SyncRoot; }
            }

            private static ILog log = LogManager.GetCurrentClassLogger();
            public object GetOrAdd(string key, Func<string, object> fac)
            {
                //if (HttpContext.Current.Session == null)
                //{
                //    log.Warn("Session Is null!!!");
                //    if (HttpContext.Current.Items[key] != null)
                //        return HttpContext.Current.Items[key];
                //    HttpContext.Current.Items[key] = fac(key);
                //    return HttpContext.Current.Items[key];
                //}
                try
                {
                    if (HttpContextHelper.Current.Session[key] != null)
                        return HttpContextHelper.Current.Session[key];
                    HttpContextHelper.Current.Session[key] = fac(key);
                    return HttpContextHelper.Current.Session[key];
                }
                catch (Exception ex)
                {
                    if (HttpContextHelper.Current.Session == null)
                    {
                        throw new Core.Exceptions.CoreException("请确认当前调用的是一个HttpRequest请求，并且HttpContext.Current.Session已被实例化！", ex);
                    }
                    else
                        throw;
                }
            }
        }
        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <value>The session.</value>
        public ISessionState Session
        {
            get { return ThreadSingletonHelper<SessionImpl>.GetOrAdd(() => new SessionImpl()); }
        }
        /// <summary>
        /// Class RequestImpl
        /// </summary>
        class RequestImpl : IRequestState
        {
            /// <summary>
            /// Gets or sets the <see cref="System.Object"/> with the specified key.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>System.Object.</returns>
            public object this[string key]
            {
                get
                {
                    return HttpContextHelper.Current.Items[key];
                }
                set
                {
                    try
                    {

                        HttpContextHelper.Current.Items[key] = value;
                    }
                    finally
                    {
                    }

                }
            }


            /// <summary>
            /// Removes the specified key.
            /// </summary>
            /// <param name="key">The key.</param>
            public void Remove(string key)
            {
                HttpContextHelper.Current.Items.Remove(key);
            }

            /// <summary>
            /// Gets the sync root.
            /// </summary>
            /// <value>The sync root.</value>
            public object SyncRoot
            {
                get { return HttpContextHelper.Current.Items.SyncRoot; }
            }


            public object GetOrAdd(string key, Func<string, object> fac)
            {
                if (HttpContextHelper.Current.Items.Contains(key))
                    return HttpContextHelper.Current.Items[key];

                lock (HttpContextHelper.Current.Items.SyncRoot)
                {
                    if (HttpContextHelper.Current.Items.Contains(key))
                        return HttpContextHelper.Current.Items[key];
                    HttpContextHelper.Current.Items[key] = fac(key);
                }
                return HttpContextHelper.Current.Items[key];
            }
        }
        /// <summary>
        /// Gets the request.
        /// </summary>
        /// <value>The request.</value>
        public IRequestState Request
        {
            get
            {
                return ThreadSingletonHelper<RequestImpl>.GetOrAdd(() => new RequestImpl());
            }
        }
    }
}