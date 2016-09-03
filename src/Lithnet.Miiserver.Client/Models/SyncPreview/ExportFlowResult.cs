using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class ExportFlowResult : XmlObjectBase
    {
        internal ExportFlowResult(XmlNode node)
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

        public string Status
        {
            get
            {
                return this.GetValue<string>("@status");
            }
        }
      
        public FlowRule FlowRule { get; private set; }

        public override string ToString()
        {
            return this.TargetAttribute;
        }
    }
}