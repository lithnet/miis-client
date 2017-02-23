using System.Collections.Generic;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class MVReferenceAttribute : XmlObjectBase
    {
        internal MVReferenceAttribute(XmlNode node)
            : base(node)
        {
        }

        public IReadOnlyCollection<MVReferenceValue> Values => this.GetReadOnlyObjectList<MVReferenceValue>("dn-value");

        public string Name => this.GetValue<string>("@name");

        public bool Multivalued => this.GetValue<bool>("@multivalued");
    }
}
