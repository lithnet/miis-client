namespace Lithnet.Miiserver.Client
{
    using System.Collections.Generic;
    using System.Xml;

    public class MVReferenceAttribute : XmlObjectBase
    {
        internal MVReferenceAttribute(XmlNode node)
            : base(node)
        {
        }

        public IReadOnlyCollection<MVReferenceValue> Values
        {
            get
            {
                return this.GetReadOnlyObjectList<MVReferenceValue>("dn-value");
            }
        }

        public string Name
        {
            get
            {
                return this.GetValue<string>("@name");
            }
        }

        public bool Multivalued
        {
            get
            {
                return this.GetValue<bool>("@multivalued");
            }
        }
    }
}
