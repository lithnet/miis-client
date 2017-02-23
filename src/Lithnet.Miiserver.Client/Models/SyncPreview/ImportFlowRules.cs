using System;
using System.Collections.Generic;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class ImportFlowRules : XmlObjectBase
    {
        public ImportFlowRules (XmlNode node)
        :base(node)
        {
        }

        public IReadOnlyDictionary<string, ImportFlowResult> ImportFlows
        {
            get
            {
                return this.GetReadOnlyObjectDictionary<string, ImportFlowResult>("import-flow", (t) => t.TargetAttribute, StringComparer.OrdinalIgnoreCase);
            }
        }

        public string Type => this.GetValue<string>("@import-flow-type");

        public bool HasError => this.GetValue<bool>("@has-error");
    }
}
