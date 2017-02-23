using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class FlowRuleDNComponent : FlowRule
    {
        internal FlowRuleDNComponent(XmlNode node)
            :base(node)
        {
            this.Type = FlowRuleType.DNComponent;
        }

        public int DNPart => this.GetValue<int>("dn-part");

        public override string ToString()
        {
            return $"DN Component - {this.DNPart}";
        }
    }
}
