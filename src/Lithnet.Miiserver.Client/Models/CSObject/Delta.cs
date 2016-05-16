namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using System.Diagnostics;
    using System.Xml;

    public class Delta : CSEntryBase
    {
        internal Delta(XmlNode node)
            : base(node)
        {
        }

        public IReadOnlyDictionary<string, AttributeChange> Attributes
        {
            get
            {
                return this.GetReadOnlyObjectDictionary<string, AttributeChange>("attr", (t) => t.Name, StringComparer.OrdinalIgnoreCase);
            }
        }

        public DeltaOperationType Operation
        {
            get
            {
                return this.GetValue<DeltaOperationType>("@operation");
            }
        }
    }
}
