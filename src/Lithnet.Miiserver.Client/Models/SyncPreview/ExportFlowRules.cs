using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class ExportFlowRules : NodeCache
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

        public string Type
        {
            get
            {
                return this.GetValue<string>("@export-flow-type");
            }
        }

        public bool HasError
        {
            get
            {
                return this.GetValue<bool>("@has-error");
            }
        }

        public Delta Delta
        {
            get
            {
                return this.GetObject<Delta>("values/delta");
            }
        }
    }
}
