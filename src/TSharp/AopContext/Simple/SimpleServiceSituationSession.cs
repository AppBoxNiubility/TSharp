using System;
using System.Web;

using TSharp.Core.Osgi.Internal;
using TSharp.Core.Osgi;

namespace TSharp.Core.Simple
{
    partial class SimpleServiceSituation
    {
        internal SimpleLocatorWrapper CreateSessionContainer(HttpContextBase ctx)
        {
            log.Debug("创建 Session");
            SimpleLocatorWrapper container = GetRootUnity().CreateChildContainer(Level.SESSION);

            Configure(ServiceManager.SessionMap, container);
            return container;
        }

        private SimpleLocatorWrapper GetSessionUnity(HttpContextBase ctx)
        {
            return GetOrCreateSessionContainer(ctx);
        }

        private SimpleLocatorWrapper GetOrCreateSessionContainer(HttpContextBase ctx)
        {
            if (ctx.Session == null)
            {
                if (ThrowExceptionNullSessionRequest)
                    throw new NotSupportedException("Web上下文Session为null，无法支持获取会话（Session）级别容器。");
                else
                    return CreateSessionContainer(ctx);
                //return GetRootUnity();
            }
            var container = ctx.Session[CACHEKEY_SESSION_CONTAINER] as SimpleLocatorWrapper;
            if (container != null)
                return container;
            lock (ctx.Session.SyncRoot)
                if ((container = ctx.Session[CACHEKEY_SESSION_CONTAINER] as SimpleLocatorWrapper) == null)
                    ctx.Session[CACHEKEY_SESSION_CONTAINER] = container = CreateSessionContainer(ctx);
            return container;
        }
    }
}