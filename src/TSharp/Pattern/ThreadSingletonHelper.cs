using System;
using System.Threading;

namespace TSharp.Core.Pattern
{
    /// <summary>
    /// 线程单例工厂
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <author>
    /// tangjingbo
    /// </author>
    /// <remarks>
    /// tangjingbo at 2009-9-26 15:45
    /// </remarks>
    public class ThreadSingletonHelper<T> where T : class
    {
        private static readonly object sync = new object();
        [ThreadStatic]
        private static T _singleton;

        /// <summary>
        /// Gets the instance of thread.
        /// </summary>
        /// <param name="creater">The creater.</param>
        /// <returns></returns>
        public static T GetOrAdd(Func<T> creater)
        {
            if (_singleton != null)
                return _singleton;
            lock (sync)
            {
                Thread.MemoryBarrier();
                if (_singleton == null)
                {
                    _singleton = creater();
                }
                return _singleton;
            }
        }

        /// <summary>
        /// 重置线程单例变量，并返回原来的对象
        /// </summary>
        /// <param name="creater">The creater.</param>
        /// <returns></returns>
        public static T Reset(Func<T> creater)
        {
            T temp = _singleton;
            _singleton = null;
            _singleton = GetOrAdd(creater);
            return temp;
        }
    }
}