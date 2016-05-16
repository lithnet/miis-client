namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Diagnostics;

    public partial class FlowRuleConstant : FlowRule
    {
        internal FlowRuleConstant(XmlNode node)
        : base(node)
        {
            this.Type = FlowRuleType.Constant;
        }

        public string ConstantValue
        {
            get
            {
                return this.GetValue<string>("constant-value");
            }
        }

        public override string ToString()
        {
            return string.Format("Constant - {0}", this.ConstantValue);
        }
    }
}
