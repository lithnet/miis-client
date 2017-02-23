using System;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class MVReferenceValue  : XmlObjectBase
    {
        internal MVReferenceValue(XmlNode node)
            :base(node)
        {
            this.Value = node.InnerText;
        }

        public string DN => this.GetValue<string>("dn");

        public string Value { get; private set; }

        public Guid LineageID => this.GetValue<Guid>("@lineage-id");

        public Guid MAID => this.GetValue<Guid>("@lineage-ma-id");

        public DateTime? LineageTime => this.GetValue<DateTime?>("@lineage-time");
    }
}
