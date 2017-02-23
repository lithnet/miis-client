using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class InboundFlowCounters : XmlObjectBase
    {
        internal InboundFlowCounters(XmlNode node)
            :base(node)
        {
        }

        public int DisconnectorFiltered => this.GetValue<int>("disconnector-filtered");

        public int DisconnectorJoinedNoFlow => this.GetValue<int>("disconnector-joined-no-flow");

        public int DisconnectorJoinedFlow => this.GetValue<int>("disconnector-joined-flow");

        public int DisconnectorJoinedRemoveMV => this.GetValue<int>("disconnector-joined-remove-mv");

        public int DisconnectorProjectedNoFlow => this.GetValue<int>("disconnector-projected-no-flow");

        public int DisconnectorProjectedFlow => this.GetValue<int>("disconnector-projected-flow");

        public int DisconnectorProjectedRemoveMV => this.GetValue<int>("disconnector-projected-remove-mv");

        public int DisconnectedRemains => this.GetValue<int>("disconnector-remains");

        public int ConnectorFilteredRemoveMV => this.GetValue<int>("connector-filtered-remove-mv");

        public int ConnectedFilteredLeaveMV => this.GetValue<int>("connector-filtered-leave-mv");

        public int ConnectorFlow => this.GetValue<int>("connector-flow");

        public int ConnectorFlowRemoveMV => this.GetValue<int>("connector-flow-remove-mv");

        public int ConnectorNoFlow => this.GetValue<int>("connector-no-flow");

        public int ConnectorDeleteRemoveMV => this.GetValue<int>("connector-delete-remove-mv");

        public int ConnectorDeleteLeaveMV => this.GetValue<int>("connector-delete-leave-mv");

        public int ConnectorDeleteAddProcessed => this.GetValue<int>("connector-delete-add-processed");

        public int FlowFailure => this.GetValue<int>("flow-failure");

        public int TotalProjections => this.DisconnectorProjectedFlow + this.DisconnectorProjectedNoFlow + this.DisconnectorProjectedRemoveMV;

        public int TotalJoins => this.DisconnectorJoinedFlow + this.DisconnectorJoinedNoFlow + this.DisconnectorJoinedRemoveMV;

        public int TotalFilteredConnectors => this.ConnectorFilteredRemoveMV + this.ConnectedFilteredLeaveMV;

        public int TotalDeletedConnectors => this.ConnectorDeleteLeaveMV + this.ConnectorDeleteRemoveMV;

        public int TotalMVObjectDeletes => this.ConnectorFlowRemoveMV + this.ConnectorDeleteRemoveMV;
    }
}
