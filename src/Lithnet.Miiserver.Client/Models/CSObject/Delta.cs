using System;
using System.Collections.Generic;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
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

        [Serialize]
        public DeltaOperationType Operation => this.GetValue<DeltaOperationType>("@operation", "operation");
    }
}
