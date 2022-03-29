using System;
using System.Web; 
using TSharp.Core.Osgi.Internal;
using TSharp.Core.Osgi;

namespace TSharp.Core.Simple
{
	partial class SimpleServiceSituation
	{
		internal SimpleLocatorWrapper CreateRequestContainer(HttpContextBase ctx)
		{
			log.Debug("创建 Request");

			SimpleLocatorWrapper container = GetSessionUnity(ctx).CreateChildContainer(Level.REQUSET);
 
			Configure(ServiceManager.RequsetMap, container);

			return container;
		}


		private SimpleLocatorWrapper GetRequestUnity(HttpContextBase ctx)
		{
			return GetOrCreateRequestContainer(ctx);
		}

		private SimpleLocatorWrapper GetOrCreateRequestContainer(HttpContextBase ctx)
		{
			if (ctx.Items == null)
			{
				if (ThrowExceptionNullSessionRequest)
					throw new NotSupportedException("Web上下文Items为null，无法支持获取请求（Request）级别容器。");
				else
					return CreateRequestContainer(ctx);
				//返回上一级容器
				// return GetSessionUnity(ctx);
			}
			var request = ctx.Items[CACHEKEY_REQUSET_CONTAINER] as SimpleLocatorWrapper;
			if (request != null)
				return request;

			lock (ctx.Items.SyncRoot)
				if ((request = ctx.Items[CACHEKEY_REQUSET_CONTAINER] as SimpleLocatorWrapper) == null)
					ctx.Items[CACHEKEY_REQUSET_CONTAINER] = request = CreateRequestContainer(ctx);
			return request;
		}
	}
}