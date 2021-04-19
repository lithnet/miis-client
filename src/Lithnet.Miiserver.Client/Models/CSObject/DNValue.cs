using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class DNValue : XmlObjectBase
    {
        internal DNValue(XmlNode node)
            :base(node)
        {
        }

        public string DN => this.GetValue<string>("dn");

        public EncodedValue Anchor => this.GetObject<EncodedValue>("anchor");

        public AttributeValueOperation Operation => this.GetValue<AttributeValueOperation>("@operation");
    }
}
