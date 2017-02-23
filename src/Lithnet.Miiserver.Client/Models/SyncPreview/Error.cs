using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class Error : XmlObjectBase
    {
        internal Error(XmlNode node)
            : base(node)
        {
        }

        public string Code => this.GetValue<string>("@code");

        public string Type => this.GetValue<string>("@type");

        public string Diagnosis => this.GetValue<string>("diagnosis");

        public ExtensionErrorInfo ExtensionErrorInfo => this.GetObject<ExtensionErrorInfo>("extension-error-info");

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
