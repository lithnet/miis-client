namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Xml;
    using System.Collections.Generic;

    public class FlowRuleSyncRule : FlowRule
    {
        internal FlowRuleSyncRule(XmlNode node)
            : base(node)
        {
            this.Type = FlowRuleType.SyncRule;
        }

        public IReadOnlyList<string> SourceAttributes
        {
            get
            {
                return this.GetReadOnlyValueList<string>("src-attribute");
            }
        }

        public string MappingType
        {
            get
            {
                return this.GetValue<string>("@mapping-type");
            }
        }

        public Guid SyncRuleID
        {
            get
            {
                return this.GetValue<Guid>("@sync-rule-id");
            }
        }

        public Guid SyncRuleMappingID
        {
            get
            {
                return this.GetValue<Guid>("@sync-rule-mapping-id");
            }
        }

        public bool InitialFlowOnly
        {
            get
            {
                return this.GetValue<bool>("@initial-flow-only");
            }
        }

        public bool IsExistenceTest
        {
            get
            {
                return this.GetValue<bool>("@is-existence-test");
            }
        }
        
    }
}

/*
 
<sync-rule-mapping mapping-type="expression" sync-rule-id="{95444F13-333C-4732-9717-AE23D129F36B}" sync-rule-mapping-id="{95444F13-333C-4732-9717-AE23D129F36B}" initial-flow-only="true" is-existence-test="false">
    <sync-rule-value>
        <export-flow>
            <dest>dn</dest>
            <src>
                <attr />
            </src>
            <fn id="Guid" />
        </export-flow>
    </sync-rule-value>
</sync-rule-mapping>
     */
