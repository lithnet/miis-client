namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Xml;
    using System.Collections.Generic;

    public class JoinCriterion : NodeCache
    {
        internal JoinCriterion(XmlNode node)
            : base(node)
        {
        }

        public Guid ID
        {
            get
            {
                return this.GetValue<Guid>("@id");
            }
        }

        public JoinRuleResult JoinRules
        {
            get
            {
                return this.GetObject<JoinRuleResult>("search");
            }
        }

        public string ResolutionType 
        {
            get
            {
                return this.GetValue<string>("resolution/@type");
            }
        }

        public string ResolutionScriptContext
        {
            get
            {
                return this.GetValue<string>("resolution/script-context");
            }
        }
    }
}

/*
<join-criterion id="{6E783449-7A8C-4B6C-BF22-91B9E696F424}">
        <search mv-object-type="user">
          <attribute-mapping mv-attribute="ADObjectGuid">
            <direct-mapping>
              <src-attribute>objectGUID</src-attribute>
            </direct-mapping>
          </attribute-mapping>
        </search>
        <resolution type="none">
          <script-context></script-context>
        </resolution>
</join-criterion>

*/
