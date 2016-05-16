namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Diagnostics;

    public class FlowRuleDNComponent : FlowRule
    {
        internal FlowRuleDNComponent(XmlNode node)
            :base(node)
        {
            this.Type = FlowRuleType.DNComponent;
        }

        public int DNPart
        {
            get
            {
                return this.GetValue<int>("dn-part");
            }
        }

        public override string ToString()
        {
            return string.Format("DN Component - {0}", this.DNPart);
        }
    }
}
