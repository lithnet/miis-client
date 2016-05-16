namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;

    public class AttributeFlow : NodeCache
    {
        internal AttributeFlow(XmlNode node)
            : base(node)
        {
            this.SetFlowRule();
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

        public string DestinationAttribute
        {
            get
            {
                return this.GetValue<string>("@dest-attr");
            }
        }

        public string ContextID
        {
            get
            {
                return this.GetValue<string>("@context-id");
            }
        }
    }
}
