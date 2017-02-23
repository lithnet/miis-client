using System;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class MVAttributeValue : AttributeValue
    {
        internal MVAttributeValue(XmlNode node, AttributeType type)
        : base(node, type)
        {
        }

        public Guid LineageID => this.GetValue<Guid>("@lineage-id");

        public Guid MAID => this.GetValue<Guid>("@lineage-ma-id");

        public DateTime? LineageTime => this.GetValue<DateTime?>("@lineage-time");
    }
}