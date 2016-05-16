namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using System.Diagnostics;
    using System.Collections.ObjectModel;

    public class AttributeChange : Attribute
    {
        internal AttributeChange(XmlNode node)
            : base(node)
        {
        }

        public AttributeOperation Operation
        {
            get
            {
                return this.GetValue<AttributeOperation>("@operation");
            }
        }

        public override string ToString()
        {
            if (this.Operation == AttributeOperation.None)
            {
                return string.Format("{0}:{1}", this.Name, this.Values.Select(t => t.ToString()).ToCommaSeparatedString());
            }
            else
            {
                return string.Format("{0}:{1}:{2}", this.Operation, this.Name, this.Values.Select(t => t.ToString()).ToCommaSeparatedString());
            }
        }
    }
}