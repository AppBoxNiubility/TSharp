using System;
using System.Linq.Expressions;
using System.Threading;

namespace TSharp.Core.Pattern
{
    /// <summary>
    /// 单例创建工厂
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <author>
    /// tangjingbo
    /// </author> 
    public class SingletonHelper<T> where T : class
    {
        private static readonly int[] _lockObject = new int[0];
        private static T _instance;

        /// <summary>
        /// 获取单例，如果不存在则创建
        /// </summary>
        /// <param name="createrExpression">The creater expression.</param>
        /// <returns></returns>
        public static T GetOrAdd(Func<T> createrExpression)
        {
            if (_instance == null)
            {
                lock (_lockObject)
                {
                    if (_instance == null)
                    {
                        T instance = createrExpression();

                        if (instance == null)
                            throw new Exception("Creator delegate cannot return null.");

                        Thread.MemoryBarrier();
                        _instance = instance; 
                    }
                }
            }
            return _instance;
        }


        /// <summary>
        /// 返回旧对象
        /// </summary>
        /// <param name="createrExpression">The creater.</param>
        /// <returns></returns>
        public static T Reset(Func<T> createrExpression)
        {
            Thread.MemoryBarrier();
            T temp = _instance;
            Thread.MemoryBarrier();
            _instance = null;
            Thread.MemoryBarrier();
            _instance = GetOrAdd(createrExpression);
            return temp;
        }
    }
}