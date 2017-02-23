using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class JoinRuleResult : XmlObjectBase
    {
        internal JoinRuleResult(XmlNode node)
            : base(node)
        {
            this.FlowRule = FlowRule.GetFlowRule(node.SelectSingleNode("attribute-mapping"));
        }

        public string ObjectType => this.GetValue<string>("@mv-object-type");

        public string MVAttribute => this.GetValue<string>("attribute-mapping/@mv-attribute");

        public string Status => this.GetValue<string>("attribute-mapping/@status");

        public FlowRule FlowRule { get; private set; }
    }
}

/*
<search mv-object-type="user">
    <attribute-mapping mv-attribute="mObjectID" status="success">
        <direct-mapping>
            <src-attribute>mObjectID</src-attribute>
        </direct-mapping>
    </attribute-mapping>
</search>
*/
