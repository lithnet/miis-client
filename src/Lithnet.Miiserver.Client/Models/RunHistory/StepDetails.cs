using System;
using System.Collections.Generic;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class StepDetails : XmlObjectBase
    {
        internal StepDetails(XmlNode node)
            : base(node)
        {
        }

        /// <summary>
        /// Gets the date and time at which the step began
        /// </summary>
        public DateTime? StartDate => this.GetValue<DateTime?>("start-date");

        /// <summary>
        /// Gets the date and time at which the step completed
        /// </summary>
        /// <remarks>
        /// This value is present only if the step has completed
        /// </remarks>
        public DateTime? EndDate => this.GetValue<DateTime?>("end-date");

        /// <summary>
        /// Gets a non-localized string that provides the current status of the step.
        /// </summary>
        public string StepResult => this.GetValue<string>("step-result");

        public string StepResultProgress => this.GetValue<string>("step-result/@progress");

        public string StepResultFile => this.GetValue<string>("step-result/@file");

        /// <summary>
        /// Gets an XML description of the step copied from the run profile. It includes information such as step type, subtype, partition name, and management agent-specific information, such as the name of the discovery file (in the case of an XML management agent).
        /// </summary>
        public StepDescription StepDefinition => this.GetObject<StepDescription>("step-description");

        /// <summary>
        /// Gets a number (sometimes called the current export batch number) that is associated with the management agent. It is incremented each time an export run-step is run for this management agent.
        /// </summary>
        public int CurrentExportStepCounter => this.GetValue<int>("current-export-step-counter");

        /// <summary>
        /// Gets the counter of the last export run step that completed in its entirety. This number is sometimes called the last successful export batch number of the management agent.
        /// </summary>
        public int LastSuccessfulExportStepCounter => this.GetValue<int>("last-successful-export-step-counter");

        /// <summary>
        /// Gets information about the connection between the management agent and the connected directory. It can appear for both import and export runs. The management agent reports the server it connected to, and also the servers it failed to connect to. The management agent reports management agent-specific error information, which is helpful in debugging the problem.
        /// </summary>
        public MAConnection MAConnection => this.GetObject<MAConnection>("ma-connection");

        /// <summary>
        /// Gets a list of errors that were encountered. This element is present only if the step does an import against a connected directory and the management agent hit errors (as opposed to synchronization errors).
        /// </summary>
        public IReadOnlyList<MAObjectError> MADiscoveryErrors => this.GetReadOnlyObjectList<MAObjectError>("ma-discovery-errors/ma-object-error");

        /// <summary>
        /// Gets the values of some management agent discovery counters.
        /// </summary>
        public MADiscoveryCounters MADiscoveryCounters => this.GetObject<MADiscoveryCounters>("ma-discovery-counters", this.StepID);

        /// <summary>
        /// Gets a list of connector space objects that are having problems either synchronizing a connector space delta through to the metaverse and out to other connector spaces, or pushing an export delta out to the connected directory.
        /// </summary>
        public SynchronizationErrors SynchronizationErrors => this.GetObject<SynchronizationErrors>("synchronization-errors");

        /// <summary>
        /// Gets information about objects that the FIM Synchronization Service could not synchronize.
        /// </summary>
        public IReadOnlyList<MVRetryError> MVRetryErrors => this.GetReadOnlyObjectList<MVRetryError>("mv-retry-errors/retry-error");

        /// <summary>
        /// Gets objects that records the counts of provisioning and export attribute flow. If the step involves applying changes from the connector space to the metaverse, there may be one or more provisioning changes or export attribute flow. Each management agent for which provisioning or non-trivial export attribute flow occurs, then this object is present
        /// </summary>
        public IReadOnlyList<OutboundFlowCounters> OutboundFlowCounters => this.GetReadOnlyObjectList<OutboundFlowCounters>("outbound-flow-counters", this.StepID);

        /// <summary>
        /// Gets statistics on the staging of the entries that were imported. If the step involves importing from a connected directory or from a drop file, this element is present.
        /// </summary>
        public StagingCounters StagingCounters => this.GetObject<StagingCounters>("staging-counters", this.StepID);

        /// <summary>
        /// Gets information about the number of disconnectors that became connectors during this run, and the disposition of existing connectors.
        /// </summary>
        public InboundFlowCounters InboundFlowCounters => this.GetObject<InboundFlowCounters>("inbound-flow-counters", this.StepID);

        /// <summary>
        /// Gets statistics on the number of changes that are sent out. If the step type is export, this element is present.
        /// </summary>
        public ExportCounters ExportCounters => this.GetObject<ExportCounters>("export-counters", this.StepID);

        /// <summary>
        /// Gets the sequence number of the step within the run, starting with 1
        /// </summary>
        public int StepNumber => this.GetValue<int>("@step-number");

        /// <summary>
        /// Gets the unique ID for the instance of this step
        /// </summary>
        public Guid StepID => this.GetValue<Guid>("@step-id");
    }
}
