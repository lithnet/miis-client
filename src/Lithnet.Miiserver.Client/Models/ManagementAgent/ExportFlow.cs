using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class ExportFlow : NodeCache
    {
        internal ExportFlow(XmlNode node)
            :base(node)
        {
            this.FlowRule = FlowRule.GetFlowRule(node);
        }

        public string TargetAttribute
        {
            get
            {
                return this.GetValue<string>("@cd-attribute");
            }
        }

        public Guid ID
        {
            get
            {
                return this.GetValue<Guid>("@id");
            }
        }

        public bool FlowNulls
        {
            get
            {
                return !this.GetValue<bool>("@suppress-deletions");
            }
        }
      
        public FlowRule FlowRule { get; private set; }

        public override string ToString()
        {
            return this.TargetAttribute;
        }
    }
}