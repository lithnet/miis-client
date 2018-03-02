using System;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class MADiscoveryCounters : XmlObjectBase
    {
        private Guid stepID;

        internal MADiscoveryCounters(XmlNode node, Guid stepID)
            : base(node)
        {
            this.stepID = stepID;
        }

        public int FilteredDeletions => this.FilteredDeletionsDetail?.Count ?? 0;

        public CounterDetail FilteredDeletionsDetail => this.GetObject<CounterDetail>("filtered-deletions",  new object[] { this.stepID });
        
        public int FilteredObjects => this.FilteredObjectsDetail?.Count ?? 0;

        public CounterDetail FilteredObjectsDetail => this.GetObject<CounterDetail>("filtered-objects",  new object[] { this.stepID });
    }
}
