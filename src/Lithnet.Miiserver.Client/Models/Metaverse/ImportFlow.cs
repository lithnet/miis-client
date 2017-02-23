using System;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class ImportFlow : XmlObjectBase
    {
        internal ImportFlow(XmlNode node)
            : base(node)
        {
            this.SetFlowRule();
        }

        public Guid ID => this.GetValue<Guid>("@id");

        public Guid SourceMAID => this.GetValue<Guid>("@src-ma");

        public string CSObjectType => this.GetValue<string>("@cd-object-type");

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
    }
}
