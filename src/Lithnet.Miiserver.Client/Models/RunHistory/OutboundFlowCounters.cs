namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Xml;

    public class OutboundFlowCounters : NodeCache
    {
        internal OutboundFlowCounters(XmlNode node)
            :base(node)
        {
        }

        public int ProvisionedAddNoFlow
        {
            get
            {
                return this.GetValue<int>("provisioned-add-no-flow");
            }
        }

        public int ProvisionedAddFlow
        {
            get
            {
                return this.GetValue<int>("provisioned-add-flow");
            }
        }

        public int ProvisionedRenameNoFlow
        {
            get
            {
                return this.GetValue<int>("provisioned-rename-no-flow");
            }
        }

        public int ProvisionRenameFlow
        {
            get
            {
                return this.GetValue<int>("provisioned-rename-flow");
            }
        }

        public int ProvisionedDisconnect
        {
            get
            {
                return this.GetValue<int>("provisioned-disconnect");
            }
        }

        public int ConnectorFlow
        {
            get
            {
                return this.GetValue<int>("connector-flow");
            }
        }

        public int ConnectorNoFlow
        {
            get
            {
                return this.GetValue<int>("connector-no-flow");
            }
        }

        public int ProvisionedDeleteAddNoFlow
        {
            get
            {
                return this.GetValue<int>("provisioned-delete-add-no-flow");
            }
        }

        public int ProvisionedDeleteAddFlow
        {
            get
            {
                return this.GetValue<int>("provisioned-delete-add-flow");
            }
        }

        public string ManagementAgent
        {
            get
            {
                return this.GetValue<string>("@ma");
            }
        }

        public Guid MAID
        {
            get
            {
                return this.GetValue<Guid>("@ma-id");
            }
        }

        public bool HasChanges
        {
            get
            {
                return this.OutboundFlowChanges > 0;
            }
        }

        public int OutboundFlowChanges
        {
            get
            {
                return
                    this.ProvisionedAddFlow + 
                    this.ProvisionedAddNoFlow + 
                    this.ProvisionedDeleteAddFlow + 
                    this.ProvisionedDeleteAddNoFlow +
                    this.ProvisionedDisconnect + 
                    this.ProvisionedRenameNoFlow + 
                    this.ProvisionRenameFlow + 
                    this.ConnectorFlow;
            }
        }
    }
}
