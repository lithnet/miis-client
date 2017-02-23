using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class FilterConditionResult : FilterCondition
    {
        internal FilterConditionResult(XmlNode node)
            : base(node)
        {
        }

        private bool IntrinsicAttribute => this.GetValue<bool>("@intrinsic-attribute");

        public bool ConditionMet => this.GetValue<bool>("@status");
    }
}

/*
<condition cd-attribute="CompanyRuleID" intrinsic-attribute="false" operator="equality" status="false">
    <value ui-radix="10">0x5</value>
</condition>
*/
