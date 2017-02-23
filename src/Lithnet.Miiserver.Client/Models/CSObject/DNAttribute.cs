using System.Collections.Generic;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class DNAttribute : XmlObjectBase
    {
        internal DNAttribute(XmlNode node)
            : base(node)
        {
        }

        public IReadOnlyList<DNValue> Values => this.GetReadOnlyObjectList<DNValue>("dn-value");

        public string Name => this.GetValue<string>("@name");

        public AttributeOperation Operation => this.GetValue<AttributeOperation>("@operation");

        public bool Multivalued => this.GetValue<bool>("@multivalued");
    }
}
