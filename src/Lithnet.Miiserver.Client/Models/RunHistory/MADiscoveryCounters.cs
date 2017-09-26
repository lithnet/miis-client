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

        public int FilteredDeletions => this.FilteredDeletionsDetail.Count;

        public CounterDetail FilteredDeletionsDetail => this.GetObject<CounterDetail>("filtered-deletions", this.stepID);
        
        public int FilteredObjects => this.FilteredObjectsDetail.Count;

        public CounterDetail FilteredObjectsDetail => this.GetObject<CounterDetail>("filtered-objects", this.stepID);
    }
}
