using System;

namespace TSharp.Core.Pattern
{
    /// <summary>
    /// 基于连续相同Key访问的单例模型
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <author>
    /// Tang Jing bo
    /// </author>
    /// <remarks>
    /// 
    /// Created : 2011-11-24
    /// </remarks>
    public class KeySingletonHelper<TKey, TValue> where TKey : IEquatable<TKey>
    {
        private static TKey _key;
        private static TValue value;
        private static int[] sync = new int[0];
        /// <summary>
        /// Gets or add the createrExpression value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="createrExpression">The creater expression.</param>
        /// <returns></returns>
        public static TValue GetOrAdd(TKey key, Func<TKey, TValue> createrExpression)
        {
            if (key.Equals(_key))
                return value;
            lock (sync)
            {
                if (key.Equals(_key))
                    return value;
                value = createrExpression(key);
                _key = key;
                return value;
            }
        }

        /// <summary>
        /// Resets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="createrExpression">The creater expression.</param>
        /// <returns></returns>
        public static TValue Reset(TKey key, Func<TKey, TValue> createrExpression)
        {
            TValue v = value;
            value = createrExpression(key);
            _key = key;
            return v;
        }
    }
}