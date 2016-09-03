namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Xml;
    using System.Collections.Generic;

    public class FilterSet : XmlObjectBase
    {
        internal FilterSet(XmlNode node)
            : base(node)
        {
        }

        public string CDObjectType
        {
            get
            {
                return this.GetValue<string>("@cd-object-type");
            }
        }

        public string Type
        {
            get
            {
                return this.GetValue<string>("@type");
            }
        }


        public bool ImportFilter
        {
            get
            {
                return this.GetValue<string>("@import-filter") == "1";
            }
        }

        public IReadOnlyList<FilterConditions> Conditions
        {
            get
            {
                return this.GetReadOnlyObjectList<FilterConditions>("filter-alternative");
            }
        }
    }
}

/*
<filter-set import-filter="0" cd-object-type="person" type="declared">
    <filter-alternative id="{DD772BB6-7E1E-4A05-A263-0F0209B21F1D}">
        <condition cd-attribute="Lastname" operator="not-present">
            <value></value>
        </condition>
    </filter-alternative>
    <filter-alternative id="{958E33FD-3903-468F-A9F5-33FE841D384B}">
        <condition cd-attribute="EmployeeCode" operator="not-present">
            <value></value>
        </condition>
    </filter-alternative>
    <filter-alternative id="{6099728D-44E1-45B8-B3AF-408FE67D3FDE}">
        <condition cd-attribute="CompanyRuleID" operator="equality">
            <value ui-radix="10">0x1</value>
        </condition>
    </filter-alternative>
</filter-set>
*/
