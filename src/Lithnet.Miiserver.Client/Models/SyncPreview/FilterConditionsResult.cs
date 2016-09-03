namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Xml;
    using System.Collections.Generic;

    public class FilterConditionsResult : XmlObjectBase
    {
        internal FilterConditionsResult(XmlNode node)
            : base(node)
        {
        }

        public string Status
        {
            get
            {
                return this.GetValue<string>("@status");
            }
        }

        public IReadOnlyList<FilterConditionResult> Conditions
        {
            get
            {
                return this.GetReadOnlyObjectList<FilterConditionResult>("condition");
            }
        }

        public override string ToString()
        {
            return this.Status;
        }
    }
}

/*
<filter-alternative status="not-satisfied">
    <condition cd-attribute="CompanyRuleID" intrinsic-attribute="false" operator="equality" status="true">
        <value ui-radix="10">0x1</value>
    </condition>
    <condition cd-attribute="CompanyRuleID" intrinsic-attribute="false" operator="equality" status="false">
        <value ui-radix="10">0x6</value>
    </condition>
</filter-alternative>
*/
