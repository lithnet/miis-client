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

    public abstract class CSEntryBase : NodeCache
    {
        internal CSEntryBase(XmlNode node)
            : base(node)
        {
        }

        public EncodedValue Anchor
        {
            get
            {
               return this.GetObject<EncodedValue>("anchor");
            }
        }

        public EncodedValue ParentAnchor
        {
            get
            {
               return this.GetObject<EncodedValue>("parent-anchor");
            }
        }

        public IReadOnlyList<DNAttribute> DNAttributes
        {
            get
            {
                return this.GetReadOnlyObjectList<DNAttribute>("dn-attr");
            }
        }
        
        public string DN
        {
            get
            {
                return this.GetValue<string>("@dn");
            }
        }

        public override string ToString()
        {
            return this.DN;
        }
    }
}