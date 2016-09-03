using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class ImportFlow : XmlObjectBase
    {
        internal ImportFlow(XmlNode node)
            : base(node)
        {
            this.SetFlowRule();
        }

        public Guid ID
        {
            get
            {
                return this.GetValue<Guid>("@id");
            }
        }

        public Guid SourceMAID
        {
            get
            {
                return this.GetValue<Guid>("@src-ma");
            }
        }

        public string CSObjectType
        {
            get
            {
                return this.GetValue<string>("@cd-object-type");
            }
        }

        private void SetFlowRule()
        {
            XmlNode n1 = node.SelectSingleNode("constant-mapping");
            if (n1 != null)
            {
                this.FlowRule = new FlowRuleConstant(n1);
                return;
            }

            n1 = node.SelectSingleNode("direct-mapping");
            if (n1 != null)
            {
                this.FlowRule = new FlowRuleDirect(n1);
                return;
            }

            n1 = node.SelectSingleNode("dn-part-mapping");
            if (n1 != null)
            {
                this.FlowRule = new FlowRuleDNComponent(n1);
                return;
            }

            n1 = node.SelectSingleNode("scripted-mapping");
            if (n1 != null)
            {
                this.FlowRule = new FlowRuleAdvanced(n1);
                return;
            }
        }

        public FlowRule FlowRule { get; private set; }
    }
}

/*
 <import-flow src-ma="{5A4F1F37-94C7-489E-80FC-A11EE901BB53}" cd-object-type="shadowOrgUnitGroup" id="{60680A37-B0E8-49E3-A931-84955D1D7BCF}">
          <direct-mapping>
            <src-attribute>monashObjectID</src-attribute>
          </direct-mapping>
</import-flow>
*/
