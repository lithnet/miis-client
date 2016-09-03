using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class MVConfiguration : XmlObjectBase
    {
        internal MVConfiguration(XmlNode node)
            : base(node)
        {
        }

        public DsmlSchema Schema
        {
            get
            {
                return this.GetObject<DsmlSchema>("schema/dsml:dsml");
            }
        }

        public string Version
        {
            get
            {
                return this.GetValue<string>("version");
            }
        }

        public bool PasswordSyncEnabled
        {
            get
            {
                return this.GetValue<string>("password-sync/password-sync-enabled") == "1";
            }
        }

        public int PasswordChangeHistorySize
        {
            get
            {
                return this.GetValue<int>("password-change-history-size");
            }
        }

        public string RulesExtension
        {
            get
            {
                return this.GetValue<string>("extension/assembly-name");
            }
        }

        public bool RulesExtensionInSeperateProcess
        {
            get
            {
                return this.GetValue<string>("extension/application-protection") != "low";
            }
        }

        public string RulesExtensionTimeout
        {
            get
            {
                return this.GetValue<string>("extension/timeout");
            }
        }

        private string ProvisioningType
        {
            get
            {
                return this.GetValue<string>("provisioning/@type");
            }
        }

        public bool ProvisioningRulesExtensionEnabled
        {
            get
            {
                return this.ProvisioningType == "scripted" || this.ProvisioningType == "both";
            }
        }

        public bool ProvisioningSyncRulesEnabled
        {
            get
            {
                return this.ProvisioningType == "sync-rule" || this.ProvisioningType == "both";
            }
        }

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
