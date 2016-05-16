namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using System.Diagnostics;
    using System.Xml;

    public class AttributeValue : NodeCache
    {
        private AttributeType type;

        internal AttributeValue(XmlNode node, AttributeType type)
            :base (node)
        {
            this.type = type;

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

        public object Value { get; private set; }

        public long ValueInteger
        {
            get
            {
                return (long)this.Value;
            }
        }

        public bool ValueBoolean
        {
            get
            {
                return (bool)this.Value;
            }
        }

        public byte[] ValueBinary
        {
            get
            {
                return (byte[])this.Value;
            }
        }

        private void DecodeValue()
        {
            if (this.Encoding == "base64")
            {
                this.Value = Convert.FromBase64String(this.ValueString);
            }
            else if (string.IsNullOrWhiteSpace(this.Encoding))
            {
                if (this.type == AttributeType.Integer)
                {
                    this.Value = Convert.ToInt64(this.ValueString, 16);
                    this.ValueString = this.Value.ToString();
                }
                else if (this.type == AttributeType.Boolean)
                {
                    this.Value = Convert.ToBoolean(this.ValueString);
                }
                else
                {
                    this.Value = this.ValueString;
                }
            }
            else
            {
                throw new InvalidOperationException("Unknown encoding type: " + this.Encoding);
            }
        }

        public override string ToString()
        {
            return string.Format("{0}", this.ValueString);
        }
    }
}