using System.Xml;

namespace Lithnet.Miiserver.Client
{
    /// <summary>
    /// Represents an attribute flow
    /// </summary>
    public class AttributeFlow : XmlObjectBase
    {
        internal AttributeFlow(XmlNode node)
            : base(node)
        {
            this.SetFlowRule();
        }

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

        /// <summary>
        /// Gets the object that describes the flow rule
        /// </summary>
        public FlowRule FlowRule { get; private set; }

        /// <summary>
        /// Gets the destination attribute name
        /// </summary>
        public string DestinationAttribute => this.GetValue<string>("@dest-attr");

        /// <summary>
        /// Gets the flow rule context
        /// </summary>
        public string ContextID => this.GetValue<string>("@context-id");
    }
}
