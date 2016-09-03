namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Diagnostics;

    public class DNValue : XmlObjectBase
    {
        internal DNValue(XmlNode node)
            :base(node)
        {
        }

        public string DN
        {
            get
            {
                return this.GetValue<string>("dn");
            }
        }

        public EncodedValue Anchor
        {
            get
            {
                return this.GetObject<EncodedValue>("anchor");
            }
        }

        public AttributeValueOperation Operation
        {
            get
            {
                return this.GetValue<AttributeValueOperation>("operation");
            }
        }
    }
}
