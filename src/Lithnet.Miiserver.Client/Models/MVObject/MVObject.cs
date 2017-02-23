using System;
using System.Collections.Generic;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class MVObject : XmlObjectBase
    {
        internal MVObject(XmlNode node)
            :base(node)
        {
        }

        public IReadOnlyList<CSMVLink> CSMVLinks => this.GetReadOnlyObjectList<CSMVLink>("cs-mv-links/cs-mv-value");


        public IReadOnlyDictionary<string, MVAttribute> Attributes
        {
            get
            {
                return this.GetReadOnlyObjectDictionary<string, MVAttribute>("entry/attr", (t) => t.Name, StringComparer.OrdinalIgnoreCase);
            }
        }

        public string ObjectType => this.GetValue<string>("entry/primary-objectclass");

        public Guid DN => this.GetValue<Guid>("entry/@dn");

        public IReadOnlyList<MVReferenceAttribute> DNAttributes => this.GetReadOnlyObjectList<MVReferenceAttribute>("entry/dn-attr");

        public Guid ID => this.GetValue<Guid>("@id");

        public override string ToString()
        {
            return this.ID.ToString();
        }
    }
}
