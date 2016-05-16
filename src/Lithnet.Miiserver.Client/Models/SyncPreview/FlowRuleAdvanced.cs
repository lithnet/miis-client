namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Xml;
    using System.Collections.Generic;

    public class FlowRuleAdvanced : FlowRule
    {
        internal FlowRuleAdvanced(XmlNode node)
            : base(node)
        {
            this.Type = FlowRuleType.RulesExtension;
        }

        public IReadOnlyList<string> SourceAttributes
        {
            get
            {
                return this.GetReadOnlyValueList<string>("src-attribute");
            }
        }

        public string ScriptContext
        {
            get
            {
                return this.GetValue<string>("script-context");
            }
        }

        public override string ToString()
        {
            return string.Format("Rules extension - {0}", this.ScriptContext);
        }
    }
}
