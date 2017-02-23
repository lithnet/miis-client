using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public abstract class FlowRule : XmlObjectBase
    {
        internal FlowRule(XmlNode node)
            : base(node)
        {
        }

        public FlowRuleType Type { get; protected set; }

        public static FlowRule GetFlowRule(XmlNode node)
        {
            XmlNode n1 = node.SelectSingleNode("constant-mapping");
            if (n1 != null)
            {
                return new FlowRuleConstant(n1);
            }

            n1 = node.SelectSingleNode("direct-mapping");
            if (n1 != null)
            {
                return new FlowRuleDirect(n1);
            }

            n1 = node.SelectSingleNode("dn-part-mapping");
            if (n1 != null)
            {
                return new FlowRuleDNComponent(n1);
            }

            n1 = node.SelectSingleNode("scripted-mapping");
            if (n1 != null)
            {
                return new FlowRuleAdvanced(n1);
            }

            n1 = node.SelectSingleNode("sync-rule-mapping");
            if (n1 != null)
            {
                return new FlowRuleSyncRule(n1);
            }

            return null;
        }
    }
}