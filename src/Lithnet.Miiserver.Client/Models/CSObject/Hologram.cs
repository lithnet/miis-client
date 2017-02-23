using System;
using System.Collections.Generic;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class Hologram : CSEntryBase
    {
        internal Hologram(XmlNode node)
            : base(node)
        {
        }

        public IReadOnlyDictionary<string, Attribute> Attributes
        {
            get
            {
                return this.GetReadOnlyObjectDictionary<string, Attribute>("attr", (t) => t.Name, StringComparer.OrdinalIgnoreCase);
            }
        }

        public string PrimaryObjectClass => this.GetValue<string>("primary-objectclass");

        public IReadOnlyList<string> ObjectClasses => this.GetReadOnlyValueList<string>("objectclass/oc-value");
    }
}