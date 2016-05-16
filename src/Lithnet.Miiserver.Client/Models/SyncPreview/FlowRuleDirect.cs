namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Diagnostics;

    public class FlowRuleDirect : FlowRule
    {
        internal FlowRuleDirect(XmlNode node)
            :base (node)
        {
            this.Type = FlowRuleType.Direct;
        }

        public string SourceAttribute
        {
            get
            {
                return this.GetValue<string>("src-attribute");
            }
        }

        public override string ToString()
        {
            return string.Format("Direct - {0}", this.SourceAttribute);
        }
    }
}
