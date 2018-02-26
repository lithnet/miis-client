using System;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class MVLink : XmlObjectBase
    {
        internal MVLink(XmlNode node)
            : base(node)
        {
            this.MVObjectID = node.ReadInnerTextAsGuid();
        }

        public Guid LineageId => this.GetValue<Guid>("@lineage-id");

       public string LineageType => this.GetValue<string>("@lineage-type");

       public DateTime? LineageTime => this.GetValue<DateTime?>("@lineage-time");

       public Guid? MVObjectID { get; private set; }

        public override string ToString()
        {
            return this.MVObjectID?.ToString();
        }
    }
}
