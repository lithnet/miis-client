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

        public string TargetAttribute => this.GetValue<string>("@cd-attribute");

        public string Status => this.GetValue<string>("@status");

        public FlowRule FlowRule { get; private set; }

        public override string ToString()
        {
            return this.TargetAttribute;
        }
    }
}