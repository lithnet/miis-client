namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Xml;
    using System.Collections.Generic;

    public class JoinRuleResult : XmlObjectBase
    {
        internal JoinRuleResult(XmlNode node)
            : base(node)
        {
            this.FlowRule = FlowRule.GetFlowRule(node.SelectSingleNode("attribute-mapping"));
        }

        public string ObjectType
        {
            get
            {
                return this.GetValue<string>("@mv-object-type");
            }
        }

        public string MVAttribute
        {
            get
            {
                return this.GetValue<string>("attribute-mapping/@mv-attribute");
            }
        }

        public string Status
        {
            get
            {
                return this.GetValue<string>("attribute-mapping/@status");
            }
        }

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
