namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Xml;
    using System.Collections.Generic;

    public class PasswordSyncSettings : NodeCache
    {
        internal PasswordSyncSettings(XmlNode node)
            : base(node)
        {
        }

        public int MaximumRetryCount
        {
            get
            {
                return this.GetValue<int>("maximum-retry-count");
            }
        }

        public int RetryInterval
        {
            get
            {
                return this.GetValue<int>("maximum-retry-interval");
            }
        }

        public bool UnlockAccount
        {
            get
            {
                return this.GetValue<string>("unlock-account") == "1";
            }
        }

        public bool RequireSecureConnection
        {
            get
            {
                return this.GetValue<string>("allow-low-security") == "0";
            }
        }
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
