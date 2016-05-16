namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using System.Diagnostics;
    using System.Xml;
    using System.Collections.ObjectModel;
    using System.Collections;

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
    }
}