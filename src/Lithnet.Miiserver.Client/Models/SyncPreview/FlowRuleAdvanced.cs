using System.Xml;
using System.Collections.Generic;

namespace Lithnet.Miiserver.Client
{
    public class FlowRuleAdvanced : FlowRule
    {
        internal FlowRuleAdvanced(XmlNode node)
            : base(node)
        {
            this.Type = FlowRuleType.RulesExtension;
        }

        public IReadOnlyList<string> SourceAttributes => this.GetReadOnlyValueList<string>("src-attribute");

        public string ScriptContext => this.GetValue<string>("script-context");

        public override string ToString()
        {
            return $"Rules extension - {this.ScriptContext}";
        }
    }
}
