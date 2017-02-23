using System;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class RulesErrorInfoContext : XmlObjectBase
    {
        internal RulesErrorInfoContext(XmlNode node)
            : base(node)
        {
        }

        public AttributeFlow AttributeFlow => this.GetObject<AttributeFlow>("attribute-mapping");

        public Guid MAID => this.GetValue<Guid>("@ma-id");

        public string MAName => this.GetValue<string>("@ma-name");

        public string CSObjectID => this.GetValue<string>("@cs-object-id");

        public string DN => this.GetValue<string>("@dn");
    }
}
