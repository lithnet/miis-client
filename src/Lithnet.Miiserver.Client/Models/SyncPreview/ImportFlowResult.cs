﻿using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class ImportFlowResult : XmlObjectBase
    {
        internal ImportFlowResult(XmlNode node)
            :base(node)
        {
            this.SetFlowRule();
        }

        public string TargetAttribute => this.GetValue<string>("@mv-attribute");

        public string Status => this.GetValue<string>("@status");

        private void SetFlowRule()
        {
            XmlNode n1 = this.XmlNode.SelectSingleNode("constant-mapping");
            if (n1 != null)
            {
                this.FlowRule = new FlowRuleConstant(n1);
                return;
            }

            n1 = this.XmlNode.SelectSingleNode("direct-mapping");
            if (n1 != null)
            {
                this.FlowRule = new FlowRuleDirect(n1);
                return;
            }

            n1 = this.XmlNode.SelectSingleNode("dn-part-mapping");
            if (n1 != null)
            {
                this.FlowRule = new FlowRuleDNComponent(n1);
                return;
            }

            n1 = this.XmlNode.SelectSingleNode("scripted-mapping");
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
