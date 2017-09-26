using System;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class InboundFlowCounters : XmlObjectBase
    {
        private Guid stepID;

        internal InboundFlowCounters(XmlNode node, Guid stepID)
            : base(node)
        {
            this.stepID = stepID;
        }

        public int DisconnectorFiltered => this.DisconnectorFilteredDetail.Count;

        public CounterDetail DisconnectorFilteredDetail => this.GetObject<CounterDetail>("disconnector-filtered");

        public int DisconnectorJoinedNoFlow => this.DisconnectorJoinedNoFlowDetail.Count;

        public CounterDetail DisconnectorJoinedNoFlowDetail => this.GetObject<CounterDetail>("disconnector-joined-no-flow");

        public int DisconnectorJoinedFlow => this.DisconnectorJoinedFlowDetail.Count;

        public CounterDetail DisconnectorJoinedFlowDetail => this.GetObject<CounterDetail>("disconnector-joined-flow");

        public int DisconnectorJoinedRemoveMV => this.DisconnectorJoinedRemoveMVDetail.Count;

        public CounterDetail DisconnectorJoinedRemoveMVDetail => this.GetObject<CounterDetail>("disconnector-joined-remove-mv");

        public int DisconnectorProjectedNoFlow => this.DisconnectorProjectedNoFlowDetail.Count;

        public CounterDetail DisconnectorProjectedNoFlowDetail => this.GetObject<CounterDetail>("disconnector-projected-no-flow");

        public int DisconnectorProjectedFlow => this.DisconnectorProjectedFlowDetail.Count;

        public CounterDetail DisconnectorProjectedFlowDetail => this.GetObject<CounterDetail>("disconnector-projected-flow");

        public int DisconnectorProjectedRemoveMV => this.DisconnectorProjectedRemoveMVDetail.Count;

        public CounterDetail DisconnectorProjectedRemoveMVDetail => this.GetObject<CounterDetail>("disconnector-projected-remove-mv");

        public int DisconnectedRemains => this.DisconnectedRemainsDetail.Count;

        public CounterDetail DisconnectedRemainsDetail => this.GetObject<CounterDetail>("disconnector-remains");

        public int ConnectorFilteredRemoveMV => this.ConnectorFilteredRemoveMVDetail.Count;

        public CounterDetail ConnectorFilteredRemoveMVDetail => this.GetObject<CounterDetail>("connector-filtered-remove-mv");

        public int ConnectedFilteredLeaveMV => this.ConnectedFilteredLeaveMVDetail.Count;

        public CounterDetail ConnectedFilteredLeaveMVDetail => this.GetObject<CounterDetail>("connector-filtered-leave-mv");

        public int ConnectorFlow => this.ConnectorFlowDetail.Count;

        public CounterDetail ConnectorFlowDetail => this.GetObject<CounterDetail>("connector-flow");

        public int ConnectorFlowRemoveMV => this.ConnectorFlowRemoveMVDetail.Count;

        public CounterDetail ConnectorFlowRemoveMVDetail => this.GetObject<CounterDetail>("connector-flow-remove-mv");

        public int ConnectorNoFlow => this.ConnectorNoFlowDetail.Count;

        public CounterDetail ConnectorNoFlowDetail => this.GetObject<CounterDetail>("connector-no-flow");

        public int ConnectorDeleteRemoveMV => this.ConnectorDeleteRemoveMVDetail.Count;

        public CounterDetail ConnectorDeleteRemoveMVDetail => this.GetObject<CounterDetail>("connector-delete-remove-mv");

        public int ConnectorDeleteLeaveMV => this.ConnectorDeleteLeaveMVDetail.Count;

        public CounterDetail ConnectorDeleteLeaveMVDetail => this.GetObject<CounterDetail>("connector-delete-leave-mv");

        public int ConnectorDeleteAddProcessed => this.ConnectorDeleteAddProcessedDetail.Count;

        public CounterDetail ConnectorDeleteAddProcessedDetail => this.GetObject<CounterDetail>("connector-delete-add-processed");

        public int FlowFailure => this.FlowFailureDetail.Count;

        public CounterDetail FlowFailureDetail => this.GetObject<CounterDetail>("flow-failure");

        public int TotalProjections => this.DisconnectorProjectedFlow + this.DisconnectorProjectedNoFlow + this.DisconnectorProjectedRemoveMV;

        public int TotalJoins => this.DisconnectorJoinedFlow + this.DisconnectorJoinedNoFlow + this.DisconnectorJoinedRemoveMV;

        public int TotalFilteredConnectors => this.ConnectorFilteredRemoveMV + this.ConnectedFilteredLeaveMV;

        public int TotalDeletedConnectors => this.ConnectorDeleteLeaveMV + this.ConnectorDeleteRemoveMV;

        public int TotalMVObjectDeletes => this.ConnectorFlowRemoveMV + this.ConnectorDeleteRemoveMV;
    }
}
