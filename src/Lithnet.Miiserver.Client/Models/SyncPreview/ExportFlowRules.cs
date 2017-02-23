using System;
using System.Collections.Generic;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class ExportFlowRules : XmlObjectBase
    {
        public ExportFlowRules(XmlNode node)
        :base(node)
        {
        }

        public IReadOnlyDictionary<string, ExportFlowResult> ExportFlows
        {
            get
            {
                return this.GetReadOnlyObjectDictionary<string, ExportFlowResult>("export-flow", (t) => t.TargetAttribute, StringComparer.OrdinalIgnoreCase);
            }
        }

        public string Type => this.GetValue<string>("@export-flow-type");

        public bool HasError => this.GetValue<bool>("@has-error");

        public Delta Delta => this.GetObject<Delta>("values/delta");
    }
}
