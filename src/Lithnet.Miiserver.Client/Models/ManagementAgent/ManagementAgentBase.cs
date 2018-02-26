using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class ManagementAgentBase : XmlObjectBase
    {           
        protected ManagementAgentBase(XmlNode node, Guid id)
            : base(node)
        {
            this.ID = id;
        }

        public string Name => this.GetValue<string>("name");

        public string Description => this.GetValue<string>("description");

        public Guid ID { get; }

        public string ListName => this.GetValue<string>("ma-listname");

        public string Category => this.GetValue<string>("category");

        public string SubType => this.GetValue<string>("subtype");

        public string CompanyName => this.GetValue<string>("ma-companyname");

        public string Version => this.GetValue<string>("version");

        public DateTime CreationTime => this.GetValue<DateTime>("creation-time");

        public DateTime LastModificationTime => this.GetValue<DateTime>("last-modification-time");

        public bool PasswordSyncAllowed => this.GetValue<string>("password-sync-allowed") == "1";

        public IReadOnlyDictionary<string, FilterSet> ConnectorFilterRules
        {
            get
            {
                return this.GetReadOnlyObjectDictionary<string, FilterSet>("stay-disconnector/filter-set", t => t.CDObjectType, StringComparer.OrdinalIgnoreCase);
            }
        }

        public IReadOnlyDictionary<string, ProjectionClassMapping> ProjectionRules
        {
            get
            {
                return this.GetReadOnlyObjectDictionary<string, ProjectionClassMapping>("projection/class-mapping", t => t.CDObjectType, StringComparer.OrdinalIgnoreCase);
            }
        }

        public IReadOnlyList<ExportFlowSet> ExportAttributeFlows => this.GetReadOnlyObjectList<ExportFlowSet>("export-attribute-flow/export-flow-set");

        public DsmlSchema Schema => this.GetObject<DsmlSchema>("schema/dsml:dsml");

        public PasswordSyncSettings PasswordSyncTargetSettings => this.PasswordSyncAllowed ? this.GetObject<PasswordSyncSettings>("password-sync") : null;
        
        public IReadOnlyList<string> SelectedAttributes => this.GetReadOnlyValueList<string>("attribute-inclusion/attribute");

        public IReadOnlyDictionary<string, JoinProfile> JoinRules
        {
            get
            {
                return this.GetReadOnlyObjectDictionary<string, JoinProfile>("join/join-profile", t => t.ObjectType, StringComparer.OrdinalIgnoreCase);
            }
        }

        public string DeprovisioningAction => this.GetValue<string>("provisioning-cleanup/action");

        public string DeprovisioningActionType => this.GetValue<string>("provisioning-cleanup/@type");

        public string RulesExtension => this.GetValue<string>("extension/assembly-name");

        public bool RulesExtensionInSeperateProcess => this.GetValue<string>("extension/application-protection") != "low";

        public string MAArchitecture => this.GetValue<string>("controller-configuration/application-architecture");

        public bool MAInSeperateProcess => this.GetValue<string>("controller-configuration/application-protection") != "low";

        public IReadOnlyDictionary<string, Partition> Partitions => this.GetReadOnlyObjectDictionary<string, Partition>("ma-partition-data/partition", (t) => t.Name ,StringComparer.OrdinalIgnoreCase);

        public IReadOnlyDictionary<string, RunConfiguration> RunProfiles
        {
            get
            {
                return this.GetReadOnlyObjectDictionary<string, RunConfiguration>("ma-run-data/run-configuration", (t) => t.Name, StringComparer.OrdinalIgnoreCase);
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}