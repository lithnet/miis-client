namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Diagnostics;

    public class EncodedValue : NodeCache
    {
        internal EncodedValue(XmlNode node)
            : base(node)
        {
            this.ValueString = node.InnerText;
            this.DecodeValue();
        }

        private string Encoding
        {
            get
            {
                return this.GetValue<string>("@encoding");
            }
        }

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
