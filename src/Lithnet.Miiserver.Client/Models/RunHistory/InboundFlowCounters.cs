namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Diagnostics;

    public class InboundFlowCounters : NodeCache
    {
        internal InboundFlowCounters(XmlNode node)
            :base(node)
        {
        }

        public int DisconnectorFiltered
        {
            get
            {
                return this.GetValue<int>("disconnector-filtered");
            }
        }

        public int DisconnectorJoinedNoFlow
        {
            get
            {
                return this.GetValue<int>("disconnector-joined-no-flow");
            }
        }

        public int DisconnectorJoinedFlow
        {
            get
            {
                return this.GetValue<int>("disconnector-joined-flow");
            }
        }

        public int DisconnectorJoinedRemoveMV
        {
            get
            {
                return this.GetValue<int>("disconnector-joined-remove-mv");
            }
        }

        public int DisconnectorProjectedNoFlow
        {
            get
            {
                return this.GetValue<int>("disconnector-projected-no-flow");
            }
        }

        public int DisconnectorProjectedFlow
        {
            get
            {
                return this.GetValue<int>("disconnector-projected-flow");
            }
        }

        public int DisconnectorProjectedRemoveMV
        {
            get
            {
                return this.GetValue<int>("disconnector-projected-remove-mv");
            }
        }

        public int DisconnectedRemains
        {
            get
            {
                return this.GetValue<int>("disconnector-remains");
            }
        }

        public int ConnectorFilteredRemoveMV
        {
            get
            {
                return this.GetValue<int>("connector-filtered-remove-mv");
            }
        }

        public int ConnectedFilteredLeaveMV
        {
            get
            {
                return this.GetValue<int>("connector-filtered-leave-mv");
            }
        }

        public int ConnectorFlow
        {
            get
            {
                return this.GetValue<int>("connector-flow");
            }
        }

        public int ConnectorFlowRemoveMV
        {
            get
            {
                return this.GetValue<int>("connector-flow-remove-mv");
            }
        }

        public int ConnectorNoFlow
        {
            get
            {
                return this.GetValue<int>("connector-no-flow");
            }
        }

        public int ConnectorDeleteRemoveMV
        {
            get
            {
                return this.GetValue<int>("connector-delete-remove-mv");
            }
        }

        public int ConnectorDeleteLeaveMV
        {
            get
            {
                return this.GetValue<int>("connector-delete-leave-mv");
            }
        }

        public int ConnectorDeleteAddProcessed
        {
            get
            {
                return this.GetValue<int>("connector-delete-add-processed");
            }
        }

        public int FlowFailure
        {
            get
            {
                return this.GetValue<int>("flow-failure");
            }
        }

        public int TotalProjections
        {
            get
            {
                return this.DisconnectorProjectedFlow + this.DisconnectorProjectedNoFlow + this.DisconnectorProjectedRemoveMV;
            }
        }

        public int TotalJoins
        {
            get
            {
                return this.DisconnectorJoinedFlow + this.DisconnectorJoinedNoFlow + this.DisconnectorJoinedRemoveMV;
            }
        }

        public int TotalFilteredConnectors
        {
            get
            {
                return this.ConnectorFilteredRemoveMV + this.ConnectedFilteredLeaveMV;
            }
        }

        public int TotalDeletedConnectors
        {
            get
            {
                return this.ConnectorDeleteLeaveMV + this.ConnectorDeleteRemoveMV;
            }
        }

        public int TotalMVObjectDeletes
        {
            get
            {
                return this.ConnectorFlowRemoveMV + this.ConnectorDeleteRemoveMV;
            }
        }
    }
}
