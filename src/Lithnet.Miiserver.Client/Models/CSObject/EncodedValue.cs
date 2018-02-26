using System;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class EncodedValue : XmlObjectBase
    {
        internal EncodedValue(XmlNode node)
            : base(node)
        {
            this.ValueString = node.InnerText;
            this.DecodeValue();
        }

        private string Encoding => this.GetValue<string>("@encoding");

        public string ValueString { get; private set; }

        public byte[] ValueBinary { get; private set; }

        private void DecodeValue()
        {
            if (this.Encoding == "base64")
            {
                this.ValueBinary = Convert.FromBase64String(this.ValueString);
            }
            else
            {
                throw new InvalidOperationException("Unknown encoding type: " + this.Encoding);
            }
        }
    }
}
