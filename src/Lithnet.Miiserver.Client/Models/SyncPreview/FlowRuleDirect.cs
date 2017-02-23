using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class FlowRuleDirect : FlowRule
    {
        internal FlowRuleDirect(XmlNode node)
            :base (node)
        {
            this.Type = FlowRuleType.Direct;
        }

        public string SourceAttribute => this.GetValue<string>("src-attribute");

        public override string ToString()
        {
            return $"Direct - {this.SourceAttribute}";
        }
    }
}
