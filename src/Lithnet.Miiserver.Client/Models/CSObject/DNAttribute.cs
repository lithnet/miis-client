namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Diagnostics;

    public class DNAttribute : XmlObjectBase
    {
        internal DNAttribute(XmlNode node)
            : base(node)
        {
        }

        public IReadOnlyList<DNValue> Values
        {
            get
            {
                return this.GetReadOnlyObjectList<DNValue>("dn-value");
            }
        }

        public string Name
        {
            get
            {
                return this.GetValue<string>("@name");
            }
        }

        public AttributeOperation Operation
        {
            get
            {
                return this.GetValue<AttributeOperation>("@operation");
            }
        }

        public bool Multivalued
        {
            get
            {
                return this.GetValue<bool>("@multivalued");
            }
        }
    }
}
