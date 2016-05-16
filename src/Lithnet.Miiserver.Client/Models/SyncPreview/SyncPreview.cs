namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Diagnostics;

    public class SyncPreview : NodeCache
    {
        internal SyncPreview(XmlNode node)
            :base (node)
        {
        }

        public string CSOperation
        {
            get
            {
                return this.GetValue<string>("cs-operation");
            }
        }

        public string MVOperation
        {
            get
            {
                return this.GetValue<string>("mv-operation");
            }
        }

        public CSObject CSObject
        {
            get
            {
                return this.GetObject<CSObject>("cs-object");
            }
        }

        public MVObject MVObject
        {
            get
            {
                return this.GetObject<MVObject>("mv/mv-object");
            }
        }

        public ImportFlowRules ImportFlowRules
        {
            get
            {
                return this.GetObject<ImportFlowRules>("import-flow-rules/import-attribute-flow");
            }
        }

        public Delta MVChanges
        {
            get
            {
                return this.GetObject<Delta>("mv-changes/delta");
            }
        }

        public JoinCriteriaResult JoinResult
        {
            get
            {
                return this.GetObject<JoinCriteriaResult>("join-rules/join");
            }
        }

        public ProvisioningResult ProvisioningResult
        {
            get
            {
                return this.GetObject<ProvisioningResult>("provisioning-rules/provisioning");
            }
        }

        public MVDeletion MVDeletionDetails
        {
            get
            {
                return this.GetObject<MVDeletion>("mv-deletion-rules/mv-deletion");
            }
        }

        public MVRecall AttributeRecalls
        {
            get
            {
                return this.GetObject<MVRecall>("mv-recall");
            }
        }

        public FilterRules FilterRules
        {
            get
            {
                return this.GetObject<FilterRules>("stay-disconnector-rules/stay-disconnector");
            }
        }

        public IReadOnlyList<CSExport> Exports
        {
            get
            {
                return this.GetReadOnlyObjectList<CSExport>("cs-export");
            }
        }

        public Error Error
        {
            get
            {
                return this.GetObject<Error>("error");
            }
        }
    }
}
