using System;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class OutboundFlowCounters : XmlObjectBase
    {
        internal OutboundFlowCounters(XmlNode node)
            :base(node)
        {
        }

        public int ProvisionedAddNoFlow => this.GetValue<int>("provisioned-add-no-flow");

        public int ProvisionedAddFlow => this.GetValue<int>("provisioned-add-flow");

        public int ProvisionedRenameNoFlow => this.GetValue<int>("provisioned-rename-no-flow");

        public int ProvisionRenameFlow => this.GetValue<int>("provisioned-rename-flow");

        public int ProvisionedDisconnect => this.GetValue<int>("provisioned-disconnect");

        public int ConnectorFlow => this.GetValue<int>("connector-flow");

        public int ConnectorNoFlow => this.GetValue<int>("connector-no-flow");

        public int ProvisionedDeleteAddNoFlow => this.GetValue<int>("provisioned-delete-add-no-flow");

        public int ProvisionedDeleteAddFlow => this.GetValue<int>("provisioned-delete-add-flow");

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
