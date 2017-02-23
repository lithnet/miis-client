using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class FlowRuleConstant : FlowRule
    {
        internal FlowRuleConstant(XmlNode node)
        : base(node)
        {
            this.Type = FlowRuleType.Constant;
        }

        public string ConstantValue => this.GetValue<string>("constant-value");

        public override string ToString()
        {
            return $"Constant - {this.ConstantValue}";
        }
    }
}
