using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class PasswordSyncSettings : XmlObjectBase
    {
        internal PasswordSyncSettings(XmlNode node)
            : base(node)
        {
        }

        public int MaximumRetryCount => this.GetValue<int>("maximum-retry-count");

        public int RetryInterval => this.GetValue<int>("maximum-retry-interval");

        public bool UnlockAccount => this.GetValue<string>("unlock-account") == "1";

        public bool RequireSecureConnection => this.GetValue<string>("allow-low-security") == "0";
    }
}
/*
 <password-sync>
    <maximum-retry-count>10</maximum-retry-count>
    <retry-interval>60</retry-interval>
    <allow-low-security>0</allow-low-security>
    <unlock-account>1</unlock-account>
  </password-sync>
*/
