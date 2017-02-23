using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public partial class MADiscoveryCounters : XmlObjectBase
    {
        internal MADiscoveryCounters(XmlNode node)
            :base(node)
        {
        }

        public int FilteredDeletions => this.GetValue<int>("filtered-deletions");

        public int FilteredObjects => this.GetValue<int>("filtered-objects");
    }
}
