using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class ImportFlowResult : NodeCache
    {
        internal ImportFlowResult(XmlNode node)
            :base(node)
        {
            this.SetFlowRule();
        }

        public string TargetAttribute
        {
            get
            {
                return this.GetValue<string>("@mv-attribute");
            }
        }

        public string Status
        {
            get
            {
                return this.GetValue<string>("@status");
            }
        }

        private void SetFlowRule()
        {
            XmlNode n1 = node.SelectSingleNode("constant-mapping");
            if (n1 != null)
            {
                this.FlowRule = new FlowRuleConstant(n1);
                return;
            }

            n1 = node.SelectSingleNode("direct-mapping");
            if (n1 != null)
            {
                this.FlowRule = new FlowRuleDirect(n1);
                return;
            }

            n1 = node.SelectSingleNode("dn-part-mapping");
            if (n1 != null)
            {
                this.FlowRule = new FlowRuleDNComponent(n1);
                return;
            }

            n1 = node.SelectSingleNode("scripted-mapping");
            if (n1 != null)
            {
                this.FlowRule = new FlowRuleAdvanced(n1);
                return;
            }
        }

        public FlowRule FlowRule { get; private set; }

        public override string ToString()
        {
            return this.TargetAttribute;
        }
    }
}
