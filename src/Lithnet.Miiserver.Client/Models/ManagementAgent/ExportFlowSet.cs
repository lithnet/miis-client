using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class ExportFlowSet : NodeCache
    {
        internal ExportFlowSet(XmlNode node)
            : base(node)
        {
        }

        public string CDObjectType
        {
            get
            {
                return this.GetValue<string>("@cd-object-type");
            }
        }

        public string MVObjectType
        {
            get
            {
                return this.GetValue<string>("@mv-object-type");
            }
        }

        public IReadOnlyDictionary<string, ExportFlow> ExportFlows
        {
            get
            {
                return this.GetReadOnlyObjectDictionary<string, ExportFlow>("export-flow", t => t.TargetAttribute, StringComparer.OrdinalIgnoreCase);
            }
        }

        public override string ToString()
        {
            return string.Format("{0} -> {1}", this.MVObjectType, this.CDObjectType);
        }
    }
}