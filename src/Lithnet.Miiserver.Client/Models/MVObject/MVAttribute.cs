namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    public class MVAttribute : XmlObjectBase
    {
        internal MVAttribute(XmlNode node)
            : base(node)
        {
        }

        public IReadOnlyList<MVAttributeValue> Values
        {
            get
            {
                return this.GetReadOnlyObjectList<MVAttributeValue>("value", this.Type);
            }
        }

        public string Name
        {
            get
            {
                return this.GetValue<string>("@name");
            }
        }

        public AttributeType Type
        {
            get
            {
                return this.GetValue<AttributeType>("@type");
            }
        }

        public bool Multivalued
        {
            get
            {
                return this.GetValue<bool>("@multivalued");
            }
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", this.Name, this.Values.Select(t => t.ToString()).ToCommaSeparatedString());
        }
    }
}