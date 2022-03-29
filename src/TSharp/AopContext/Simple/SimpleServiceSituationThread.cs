using System;

using TSharp.Core.Osgi.Internal;
using TSharp.Core.Osgi;

namespace TSharp.Core.Simple
{
    partial class SimpleServiceSituation
    {
        private static readonly object synobj = new object();
        [ThreadStatic]
        private static SimpleLocatorWrapper threadContainer;

        internal SimpleLocatorWrapper CreateThreadContainer()
        {
            log.Debug("创建 Thread");

            SimpleLocatorWrapper container = GetRootUnity().CreateChildContainer(Level.THREAD);



            Configure(ServiceManager.ThreadMap, container);
            return container;
        }

        private SimpleLocatorWrapper GetThreadUnity()
        {
            return GetOrCreateThreadContainer();
        }

        private SimpleLocatorWrapper GetOrCreateThreadContainer()
        {
            if (threadContainer != null)
                return threadContainer;
            lock (synobj)
            {
                if (threadContainer == null)
                    threadContainer = CreateThreadContainer();
                return threadContainer;
            }
        }
    }
}