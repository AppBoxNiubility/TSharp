namespace TSharp.Core.Message
{
    /// <summary>
    ///     消息处理机制情景上下文工厂
    /// </summary>
    public interface ISituationFactory
    {
        /// <summary>
        ///     Creates this instance.
        /// </summary>
        /// <returns>ISituation.</returns>
        ISituation GetSituation();

        /// <summary>
        ///     Creates the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <returns>ISituation.</returns>
        ISituation GetSituation(object sender);
    }
}