#region

using TSharp.Core.Message;
using TSharp.Core.Message.MessageImpl;
using TSharp.Core.Osgi;

#endregion

[assembly: RegLazyLoading(typeof (ISituationFactory), typeof (SituationFactory))]

namespace TSharp.Core.Message
{
    internal class SituationFactory : ISituationFactory
    {
        #region ISituationFactory 成员

        private static readonly string Key = "_tjb.situation.factory." + typeof (SituationFactory).FullName;

        public ISituation GetSituation()
        {
            return AopContext.GetContext().Application.GetOrAdd(Key, k => new SituationImpl()) as ISituation;
        }

        public ISituation GetSituation(object sender)
        {
            if (sender == null)
                return GetSituation();
            return
                AopContext.GetContext()
                          .Application.GetOrAdd(Key + "." + sender.GetType().FullName, k => new SituationImpl(sender)) as
                ISituation;
        }

        #endregion
    }
}