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

        public int DisconnectorFiltered => this.DisconnectorFilteredDetail?.Count ?? 0;

        public CounterDetail DisconnectorFilteredDetail => this.GetObject<CounterDetail>("disconnector-filtered", this.stepID);

        public int DisconnectorJoinedNoFlow => this.DisconnectorJoinedNoFlowDetail?.Count ?? 0;

        public CounterDetail DisconnectorJoinedNoFlowDetail => this.GetObject<CounterDetail>("disconnector-joined-no-flow", this.stepID);

        public int DisconnectorJoinedFlow => this.DisconnectorJoinedFlowDetail?.Count ?? 0;

        public CounterDetail DisconnectorJoinedFlowDetail => this.GetObject<CounterDetail>("disconnector-joined-flow", this.stepID);

        public int DisconnectorJoinedRemoveMV => this.DisconnectorJoinedRemoveMVDetail?.Count ?? 0;

        public CounterDetail DisconnectorJoinedRemoveMVDetail => this.GetObject<CounterDetail>("disconnector-joined-remove-mv", this.stepID);

        public int DisconnectorProjectedNoFlow => this.DisconnectorProjectedNoFlowDetail?.Count ?? 0;

        public CounterDetail DisconnectorProjectedNoFlowDetail => this.GetObject<CounterDetail>("disconnector-projected-no-flow", this.stepID);

        public int DisconnectorProjectedFlow => this.DisconnectorProjectedFlowDetail?.Count ?? 0;

        public CounterDetail DisconnectorProjectedFlowDetail => this.GetObject<CounterDetail>("disconnector-projected-flow", this.stepID);

        public int DisconnectorProjectedRemoveMV => this.DisconnectorProjectedRemoveMVDetail?.Count ?? 0;

        public CounterDetail DisconnectorProjectedRemoveMVDetail => this.GetObject<CounterDetail>("disconnector-projected-remove-mv", this.stepID);

        public int DisconnectedRemains => this.DisconnectedRemainsDetail?.Count ?? 0;

        public CounterDetail DisconnectedRemainsDetail => this.GetObject<CounterDetail>("disconnector-remains", this.stepID);

        public int ConnectorFilteredRemoveMV => this.ConnectorFilteredRemoveMVDetail?.Count ?? 0;

        public CounterDetail ConnectorFilteredRemoveMVDetail => this.GetObject<CounterDetail>("connector-filtered-remove-mv", this.stepID);

        public int ConnectedFilteredLeaveMV => this.ConnectedFilteredLeaveMVDetail?.Count ?? 0;

        public CounterDetail ConnectedFilteredLeaveMVDetail => this.GetObject<CounterDetail>("connector-filtered-leave-mv", this.stepID);

        public int ConnectorFlow => this.ConnectorFlowDetail?.Count ?? 0;

        public CounterDetail ConnectorFlowDetail => this.GetObject<CounterDetail>("connector-flow", this.stepID);

        public int ConnectorFlowRemoveMV => this.ConnectorFlowRemoveMVDetail?.Count ?? 0;

        public CounterDetail ConnectorFlowRemoveMVDetail => this.GetObject<CounterDetail>("connector-flow-remove-mv", this.stepID);

        public int ConnectorNoFlow => this.ConnectorNoFlowDetail?.Count ?? 0;

        public CounterDetail ConnectorNoFlowDetail => this.GetObject<CounterDetail>("connector-no-flow", this.stepID);

        public int ConnectorDeleteRemoveMV => this.ConnectorDeleteRemoveMVDetail?.Count ?? 0;

        public CounterDetail ConnectorDeleteRemoveMVDetail => this.GetObject<CounterDetail>("connector-delete-remove-mv", this.stepID);

        public int ConnectorDeleteLeaveMV => this.ConnectorDeleteLeaveMVDetail?.Count ?? 0;

        public CounterDetail ConnectorDeleteLeaveMVDetail => this.GetObject<CounterDetail>("connector-delete-leave-mv", this.stepID);

        public int ConnectorDeleteAddProcessed => this.ConnectorDeleteAddProcessedDetail?.Count ?? 0;

        public CounterDetail ConnectorDeleteAddProcessedDetail => this.GetObject<CounterDetail>("connector-delete-add-processed", this.stepID);

        public int FlowFailure => this.FlowFailureDetail?.Count ?? 0;

        public CounterDetail FlowFailureDetail => this.GetObject<CounterDetail>("flow-failure", this.stepID);

        public int TotalProjections => this.DisconnectorProjectedFlow + this.DisconnectorProjectedNoFlow + this.DisconnectorProjectedRemoveMV;

        public int TotalJoins => this.DisconnectorJoinedFlow + this.DisconnectorJoinedNoFlow + this.DisconnectorJoinedRemoveMV;

        public int TotalFilteredConnectors => this.ConnectorFilteredRemoveMV + this.ConnectedFilteredLeaveMV;

        public int TotalDeletedConnectors => this.ConnectorDeleteLeaveMV + this.ConnectorDeleteRemoveMV;

        public int TotalMVObjectDeletes => this.ConnectorFlowRemoveMV + this.ConnectorDeleteRemoveMV;
    }
}
