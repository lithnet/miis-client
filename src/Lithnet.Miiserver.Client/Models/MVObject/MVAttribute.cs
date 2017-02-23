using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class MVAttribute : XmlObjectBase
    {
        internal MVAttribute(XmlNode node)
            : base(node)
        {
        }

        public IReadOnlyList<MVAttributeValue> Values => this.GetReadOnlyObjectList<MVAttributeValue>("value", this.Type);

        public string Name => this.GetValue<string>("@name");

        public AttributeType Type => this.GetValue<AttributeType>("@type");

        public bool Multivalued => this.GetValue<bool>("@multivalued");

        public override string ToString()
        {
            return string.Format("{0}:{1}", this.Name, this.Values.Select(t => t.ToString()).ToCommaSeparatedString());
        }
    }
}