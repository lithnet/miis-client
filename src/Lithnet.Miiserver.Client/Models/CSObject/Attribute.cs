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

    public class Attribute : NodeCache
    {
        private IReadOnlyList<object> values;

        internal Attribute(XmlNode node)
            : base(node)
        {
        }

        private IReadOnlyList<AttributeValue> ValuesInternal
        {
            get
            {
                return this.GetReadOnlyObjectList<AttributeValue>("value", this.Type);
            }
        }

        public IReadOnlyList<object> Values
        {
            get
            {
                if (this.values == null)
                {
                    this.values = this.ValuesInternal.Select(t => t.Value).ToList().AsReadOnly();
                }

                return this.values;
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