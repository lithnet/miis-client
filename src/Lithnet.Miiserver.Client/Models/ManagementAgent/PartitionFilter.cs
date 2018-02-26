using System.Collections.Generic;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class PartitionFilter : XmlObjectBase
    {
        internal PartitionFilter(XmlNode node)
            : base(node)
        {
        }

        public IReadOnlyList<string> ObjectClasses => this.GetReadOnlyValueList<string>("object-classes/object-class");

        public IReadOnlyList<string> ContainerExclusions => this.GetReadOnlyValueList<string>("containers/exclusions/exclusion");

        public IReadOnlyList<string> ContainerInclusions => this.GetReadOnlyValueList<string>("containers/inclusions/inclusion");
    }
}
