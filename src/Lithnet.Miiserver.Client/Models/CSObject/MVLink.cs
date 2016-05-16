namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Diagnostics;
    using System.Xml.Serialization;

    public class MVLink : NodeCache
    {
        internal MVLink(XmlNode node)
            : base(node)
        {
            this.MVObjectID = node.ReadInnerTextAsGuid();
        }

        public Guid LineageId
        {
            get
            {
                return this.GetValue<Guid>("@lineage-id");
            }
        }

        public string LineageType
        {
            get
            {
                return this.GetValue<string>("@lineage-type");
            }
        }

        public DateTime? LineageTime
        {
            get
            {
                return this.GetValue<DateTime?>("@lineage-time");
            }
        }

        public Guid? MVObjectID { get; private set; }

        public override string ToString()
        {
            return MVObjectID?.ToString();
        }
    }
}
