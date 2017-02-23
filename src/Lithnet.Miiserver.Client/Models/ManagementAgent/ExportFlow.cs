using System;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class ExportFlow : XmlObjectBase
    {
        internal ExportFlow(XmlNode node)
            :base(node)
        {
            this.FlowRule = FlowRule.GetFlowRule(node);
        }

        public string TargetAttribute => this.GetValue<string>("@cd-attribute");

        public Guid ID => this.GetValue<Guid>("@id");

        public bool FlowNulls => !this.GetValue<bool>("@suppress-deletions");

        public FlowRule FlowRule { get; private set; }

        public override string ToString()
        {
            return this.TargetAttribute;
        }
    }
}