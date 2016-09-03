namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Xml;
    using System.Collections.Generic;

    public class JoinCriterionResult : XmlObjectBase
    {
        internal JoinCriterionResult(XmlNode node)
            : base(node)
        {
        }

        public string Type
        {
            get
            {
                return this.GetValue<string>("@join-cri-type");
            }
        }

        public string Status
        {
            get
            {
                return this.GetValue<string>("@status");
            }
        }

        public IReadOnlyList<MVObject> Matches
        {
            get
            {
                return this.GetReadOnlyObjectList<MVObject>("search-results/mv-object");
            }
        }

        public IReadOnlyList<Attribute> SearchValues
        {
            get
            {
                return this.GetReadOnlyObjectList<Attribute>("search-values/entry/attr");
            }
        }

        public string ResolutionStatus
        {
            get
            {
                return this.GetValue<string>("resolution/@status");
            }
        }

        public JoinRuleResult JoinRules
        {
            get
            {
                return this.GetObject<JoinRuleResult>("search");
            }
        }

        public override string ToString()
        {
            return this.Status;
        }

    }
}

/*
<join-criterion join-cri-type="none" status="no-match">
    <search mv-object-type="user">
        <attribute-mapping mv-attribute="acmaObjectID" status="not-satisfied">
            <direct-mapping>
                <src-attribute>adminDescription</src-attribute>
            </direct-mapping>
        </attribute-mapping>
    </search>
</join-criterion>
 
<join-criterion join-cri-type="none" status="found-match">
    <search mv-object-type="user">
        <attribute-mapping mv-attribute="mObjectID" status="success">
            <direct-mapping>
                <src-attribute>mObjectID</src-attribute>
            </direct-mapping>
        </attribute-mapping>
    </search>
    <search-values>
        <entry>
            <attr name="mObjectID" type="string" multivalued="false">
                <value>78669608-ca4b-4e39-b7ef-068d293ca2f7</value>
            </attr>
        </entry>
    </search-values>
    <search-results>
        <mv-object id="{96266C9C-F31E-E511-BD75-005056BA23AA}">
            <entry dn="{96266C9C-F31E-E511-BD75-005056BA23AA}">
                <primary-objectclass lineage-id="{27DC932F-1EFC-49E5-AB50-4431EB39B9A0}" lineage-time="2015-06-30 06:45:44.337">user</primary-objectclass>
                <dn-attr name="facultyPortfolio" multivalued="false">
                <dn-value lineage-id="{2FD4064E-7740-46E3-9BAE-38A9BC40E207}" lineage-time="2015-10-07 07:25:47.720">
                    <dn>{7224DC70-F31E-E511-BD75-005056BA23AA}</dn>
                </dn-value>
                </dn-attr>
                <dn-attr name="mgrAccount" multivalued="false">
                <dn-value lineage-id="{E4CED56C-68CA-4F38-92B6-039D9BF48207}" lineage-time="2015-10-07 07:25:47.723">
                    <dn>{DBD00DA6-F81E-E511-BD75-005056BA23AA}</dn>
                </dn-value>
                </dn-attr>
                <dn-attr name="organizationalUnit" multivalued="false">
                <dn-value lineage-id="{0B949E27-CC90-4D8D-A5FC-F2F597E2EBFE}" lineage-time="2015-10-07 07:25:47.727">
                    <dn>{38FFD676-F31E-E511-BD75-005056BA23AA}</dn>
                </dn-value>
                </dn-attr>
                <dn-attr name="owner" multivalued="false">
                <dn-value lineage-id="{CB945A45-686C-4BA7-9266-C01176D5CD81}" lineage-time="2015-06-30 09:25:24.913">
                    <dn>{96266C9C-F31E-E511-BD75-005056BA23AA}</dn>
                </dn-value>
                </dn-attr>
                <dn-attr name="sapOrganizationalUnit" multivalued="false">
                <dn-value lineage-id="{093E2F9E-8EA7-423F-8256-32D310477CCA}" lineage-time="2015-06-30 06:45:44.340">
                    <dn>{38FFD676-F31E-E511-BD75-005056BA23AA}</dn>
                </dn-value>
                </dn-attr>
                <dn-attr name="schoolDivision" multivalued="false">
                <dn-value lineage-id="{F119D26A-2149-4F16-BA5A-5512B70B650F}" lineage-time="2015-10-07 07:25:47.730">
                    <dn>{0125DC70-F31E-E511-BD75-005056BA23AA}</dn>
                </dn-value>
                </dn-attr>
                <dn-attr name="supervisor" multivalued="false">
                <dn-value lineage-id="{60B043E4-702A-4FED-BBE6-ECE99FC7B8B8}" lineage-time="2015-08-09 09:52:35.403">
                    <dn>{96266C9C-F31E-E511-BD75-005056BA23AA}</dn>
                </dn-value>
                </dn-attr>
                <attr name="accountDisabled" type="boolean" multivalued="false">
                <value lineage-id="{2D77B7C7-FBCA-4166-A05C-8A922A0C9AEC}" lineage-time="2015-10-07 07:25:47.720">false</value>
                </attr>
                <attr name="accountExpired" type="boolean" multivalued="false">
                <value lineage-id="{A7C58E96-B983-4AD7-B818-7E11DCAAFA06}" lineage-time="2015-10-07 07:25:47.720">false</value>
                </attr>
                <attr name="accountName" type="string" multivalued="false">
                <value lineage-id="{07A05B64-B2A7-4351-B63B-1337C1824FF5}" lineage-time="2015-10-07 07:25:47.720">rnewing</value>
                </attr>
            </entry>
        </mv-object>
    </search-results>
    <resolution status="found-match"></resolution>
</join-criterion>

*/
