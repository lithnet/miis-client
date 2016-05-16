namespace Lithnet.Miiserver.Client
{
    using System.Xml;

    public partial class MADiscoveryCounters : NodeCache
    {
        internal MADiscoveryCounters(XmlNode node)
            :base(node)
        {
        }

        public int FilteredDeletions
        {
            get
            {
                return this.GetValue<int>("filtered-deletions");
            }
        }

        public int FilteredObjects
        {
            get
            {
                return this.GetValue<int>("filtered-objects");
            }
        }
    }
}
