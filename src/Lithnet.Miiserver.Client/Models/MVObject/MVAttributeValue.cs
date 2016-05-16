namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Diagnostics;

    public class MVAttributeValue : AttributeValue
    {
        internal MVAttributeValue(XmlNode node, AttributeType type)
        : base(node, type)
        {
        }

        public Guid LineageID
        {
            get
            {
                return this.GetValue<Guid>("@lineage-id");
            }
        }

        public Guid MAID
        {
            get
            {
                return this.GetValue<Guid>("@lineage-ma-id");
            }
        }

        public DateTime? LineageTime
        {
            get
            {
                return this.GetValue<DateTime?>("@lineage-time");
            }
        }
    }
}