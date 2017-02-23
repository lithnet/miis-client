using System;
using System.Collections.Generic;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class MVConfiguration : XmlObjectBase
    {
        internal MVConfiguration(XmlNode node)
            : base(node)
        {
        }

        public DsmlSchema Schema => this.GetObject<DsmlSchema>("schema/dsml:dsml");

        public string Version => this.GetValue<string>("version");

        public bool PasswordSyncEnabled => this.GetValue<string>("password-sync/password-sync-enabled") == "1";

        public int PasswordChangeHistorySize => this.GetValue<int>("password-change-history-size");

        public string RulesExtension => this.GetValue<string>("extension/assembly-name");

        public bool RulesExtensionInSeperateProcess => this.GetValue<string>("extension/application-protection") != "low";

        public string RulesExtensionTimeout => this.GetValue<string>("extension/timeout");

        private string ProvisioningType => this.GetValue<string>("provisioning/@type");

        public bool ProvisioningRulesExtensionEnabled => this.ProvisioningType == "scripted" || this.ProvisioningType == "both";

        public bool ProvisioningSyncRulesEnabled => this.ProvisioningType == "sync-rule" || this.ProvisioningType == "both";

        public IReadOnlyDictionary<string, ImportFlowSet> ImportFlowSets
        {
            get
            {
                return this.GetReadOnlyObjectDictionary<string, ImportFlowSet>("import-attribute-flow/import-flow-set", t => t.MVObjectType, StringComparer.OrdinalIgnoreCase);
            }
        }

        public IReadOnlyDictionary<string, MVDeletionRule> MVDeletionRules
        {
            get
            {
                return this.GetReadOnlyObjectDictionary<string, MVDeletionRule>("mv-deletion/mv-deletion-rule", t => t.ObjectType, StringComparer.OrdinalIgnoreCase);
            }
        }
    }
}
