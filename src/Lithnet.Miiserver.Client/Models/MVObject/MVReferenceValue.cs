namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Xml;

    public class MVReferenceValue  : NodeCache
    {
        internal MVReferenceValue(XmlNode node)
            :base(node)
        {
            this.Value = node.InnerText;
        }

        public string DN
        {
            get
            {
                return this.GetValue<string>("dn");
            }
        }

        public string Value { get; private set; }

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
