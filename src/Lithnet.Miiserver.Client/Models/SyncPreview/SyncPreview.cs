using System.Collections.Generic;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class SyncPreview : XmlObjectBase
    {
        internal SyncPreview(XmlNode node)
            :base (node)
        {
        }

        public string CSOperation => this.GetValue<string>("cs-operation");

        public string MVOperation => this.GetValue<string>("mv-operation");

        public CSObject CSObject => this.GetObject<CSObject>("cs-object");

        public MVObject MVObject => this.GetObject<MVObject>("mv/mv-object");

        public ImportFlowRules ImportFlowRules => this.GetObject<ImportFlowRules>("import-flow-rules/import-attribute-flow");

        public Delta MVChanges => this.GetObject<Delta>("mv-changes/delta");

        public JoinCriteriaResult JoinResult => this.GetObject<JoinCriteriaResult>("join-rules/join");

        public ProvisioningResult ProvisioningResult => this.GetObject<ProvisioningResult>("provisioning-rules/provisioning");

        public MVDeletion MVDeletionDetails => this.GetObject<MVDeletion>("mv-deletion-rules/mv-deletion");

        public MVRecall AttributeRecalls => this.GetObject<MVRecall>("mv-recall");

        public FilterRules FilterRules => this.GetObject<FilterRules>("stay-disconnector-rules/stay-disconnector");

        public IReadOnlyList<CSExport> Exports => this.GetReadOnlyObjectList<CSExport>("cs-export");

        public Error Error => this.GetObject<Error>("error");
    }
}
