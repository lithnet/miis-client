namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Xml;
    using System.Collections.Generic;

    public class Error : NodeCache
    {
        internal Error(XmlNode node)
            : base(node)
        {
        }

        public string Code
        {
            get
            {
                return this.GetValue<string>("@code");
            }
        }

        public string Type
        {
            get
            {
                return this.GetValue<string>("@type");
            }
        }

        public string Diagnosis
        {
            get
            {
                return this.GetValue<string>("diagnosis");
            }
        }

        public ExtensionErrorInfo ExtensionErrorInfo
        {
            get
            {
                return this.GetObject<ExtensionErrorInfo>("extension-error-info");
            }
        }

        public override string ToString()
        {
            return this.Type;
        }
    }
}

/*
<error type="connector-filter-rule-violation" code="0x8023021c">
    <diagnosis>CS to MV to CS synchronization failed 0x8023021c: [21]</diagnosis>
</error>
*/
