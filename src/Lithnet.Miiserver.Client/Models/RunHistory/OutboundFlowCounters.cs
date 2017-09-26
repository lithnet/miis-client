using System;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class OutboundFlowCounters : XmlObjectBase
    {
        private Guid stepID;

        internal OutboundFlowCounters(XmlNode node, Guid stepID)
            : base(node)
        {
            this.stepID = stepID;
        }

        public int ProvisionedAddNoFlow => this.ProvisionedAddNoFlowDetail.Count;

        public CounterDetail ProvisionedAddNoFlowDetail => this.GetObject<CounterDetail>("provisioned-add-no-flow", this.stepID);

        public int ProvisionedAddFlow => this.ProvisionedAddFlowDetail.Count;

        public CounterDetail ProvisionedAddFlowDetail => this.GetObject<CounterDetail>("provisioned-add-flow", this.stepID);
        
        public int ProvisionedRenameNoFlow => this.ProvisionedRenameNoFlowDetail.Count;

        public CounterDetail ProvisionedRenameNoFlowDetail => this.GetObject<CounterDetail>("provisioned-rename-no-flow", this.stepID);
        
        public int ProvisionRenameFlow => this.ProvisionRenameFlowDetail.Count;

        public CounterDetail ProvisionRenameFlowDetail => this.GetObject<CounterDetail>("provisioned-rename-flow", this.stepID);
        
        public int ProvisionedDisconnect => this.ProvisionedDisconnectDetail.Count;

        public CounterDetail ProvisionedDisconnectDetail => this.GetObject<CounterDetail>("provisioned-disconnect", this.stepID);
        
        public int ConnectorFlow => this.ConnectorFlowDetail.Count;

        public CounterDetail ConnectorFlowDetail => this.GetObject<CounterDetail>("connector-flow", this.stepID);

        public int ConnectorNoFlow => this.ConnectorNoFlowDetail.Count;

        public CounterDetail ConnectorNoFlowDetail => this.GetObject<CounterDetail>("connector-no-flow", this.stepID);

        public int ProvisionedDeleteAddNoFlow => this.ProvisionedDeleteAddNoFlowDetail.Count;

        public CounterDetail ProvisionedDeleteAddNoFlowDetail => this.GetObject<CounterDetail>("provisioned-delete-add-no-flow", this.stepID);

        public int ProvisionedDeleteAddFlow => this.ProvisionedDeleteAddFlowDetail.Count;

        public CounterDetail ProvisionedDeleteAddFlowDetail => this.GetObject<CounterDetail>("provisioned-delete-add-flow", this.stepID);

        public string ManagementAgent => this.GetValue<string>("@ma");

        public Guid MAID => this.GetValue<Guid>("@ma-id");

        public bool HasChanges => this.OutboundFlowChanges > 0;

        public int OutboundFlowChanges => this.ProvisionedAddFlow + 
                                          this.ProvisionedAddNoFlow + 
                                          this.ProvisionedDeleteAddFlow + 
                                          this.ProvisionedDeleteAddNoFlow +
                                          this.ProvisionedDisconnect + 
                                          this.ProvisionedRenameNoFlow + 
                                          this.ProvisionRenameFlow + 
                                          this.ConnectorFlow;
    }
}
