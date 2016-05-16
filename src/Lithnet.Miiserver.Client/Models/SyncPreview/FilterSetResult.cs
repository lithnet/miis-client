namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Xml;
    using System.Collections.Generic;

    public class FilterSetResult : NodeCache
    {
        internal FilterSetResult(XmlNode node)
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

        public string Type
        {
            get
            {
                return this.GetValue<string>("@type");
            }
        }


        public IReadOnlyList<FilterConditionsResult> Conditions
        {
            get
            {
                return this.GetReadOnlyObjectList<FilterConditionsResult>("filter-alternative");
            }
        }
    }
}

/*
<filter-set type="declared" status="not-satisfied">
    <filter-alternative status="not-satisfied">
        <condition cd-attribute="Lastname" intrinsic-attribute="false" operator="not-present" status="false">
        <value />
        </condition>
    </filter-alternative>
    <filter-alternative status="not-satisfied">
        <condition cd-attribute="EmployeeCode" intrinsic-attribute="false" operator="not-present" status="false">
        <value />
        </condition>
    </filter-alternative>
    <filter-alternative status="not-satisfied">
        <condition cd-attribute="DepartmentCode" intrinsic-attribute="false" operator="not-present" status="false">
        <value />
        </condition>
    </filter-alternative>
    <filter-alternative status="not-satisfied">
        <condition cd-attribute="CompanyRuleID" intrinsic-attribute="false" operator="equality" status="true">
        <value ui-radix="10">0x1</value>
        </condition>
        <condition cd-attribute="CompanyRuleID" intrinsic-attribute="false" operator="equality" status="false">
        <value ui-radix="10">0x6</value>
        </condition>
    </filter-alternative>
    <filter-alternative status="not-satisfied">
        <condition cd-attribute="CompanyRuleID" intrinsic-attribute="false" operator="equality" status="false">
        <value ui-radix="10">0x5</value>
        </condition>
    </filter-alternative>
</filter-set>
*/
