namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Xml;

    public class FilterConditionResult : FilterCondition
    {
        internal FilterConditionResult(XmlNode node)
            : base(node)
        {
        }

        private bool IntrinsicAttribute
        {
            get
            {
                return this.GetValue<bool>("@intrinsic-attribute");
            }
        }

        public bool ConditionMet
        {
            get
            {
                return this.GetValue<bool>("@status");
            }
        }
    }
}

/*
<condition cd-attribute="CompanyRuleID" intrinsic-attribute="false" operator="equality" status="false">
    <value ui-radix="10">0x5</value>
</condition>
*/
