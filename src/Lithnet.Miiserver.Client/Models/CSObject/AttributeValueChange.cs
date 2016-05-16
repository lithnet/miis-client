namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using System.Diagnostics;
    using System.Xml;

    public class AttributeValueChange : AttributeValue
    {
        internal AttributeValueChange(XmlNode node, AttributeType type)
            :base(node, type)
        {
        }

        public AttributeValueOperation Operation
        {
            get
            {
                return this.GetValue<AttributeValueOperation>("@operation");
            }
        }

        public override string ToString()
        {
            if (this.Operation == AttributeValueOperation.None)
            {
                return string.Format("{0}", this.ValueString);
            }
            else
            {
                return string.Format("{0}: {1}", this.Operation, this.ValueString);
            }
        }
    }
}
