using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class xMVEntry : XmlObjectBase
    {
        internal xMVEntry(XmlNode node)
            :base(node)
        {
        }

        public IReadOnlyDictionary<string, MVAttribute> Attributes
        {
            get
            {
                return this.GetReadOnlyObjectDictionary<string, MVAttribute>("attr", (t) => t.Name, StringComparer.OrdinalIgnoreCase);
            }
        }

        public string PrimaryObjectClass
        {
            get
            {
                return this.GetValue<string>("primary-objectclass");
            }
        }

        public IReadOnlyList<string> ObjectClasses
        {
            get
            {
                return this.GetReadOnlyValueList<string>("objectclass/oc-value");
            }
        }

        public Guid DN
        {
            get
            {
                return this.GetValue<Guid>("@dn");
            }
        }

        public IReadOnlyList<MVReferenceAttribute> DNAttributes
        {
            get
            {
                return this.GetReadOnlyObjectList<MVReferenceAttribute>("dn-attr");
            }
        }
    }
}