namespace TSharp.Core.Pattern
{
    /// <summary>
    /// 基于连续相同Key访问的单例模型
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <author>
    /// Tang Jing bo
    /// </author>
    /// <remarks>
    /// Created : 2011-11-24
    /// </remarks>
    public class StringSingletonHelper<TValue> : KeySingletonHelper<string, TValue>
    {

    }
}