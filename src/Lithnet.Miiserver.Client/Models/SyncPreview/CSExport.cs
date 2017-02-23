using System;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class CSExport : XmlObjectBase
    {
        internal CSExport(XmlNode node)
            : base(node)
        {
        }

        public ExportChange BeforeChange => this.GetObject<ExportChange>("export-before-change");

        public Guid? MAID => this.BeforeChange?.MAID ?? this.AfterChange?.MAID;

        public Guid? ID => this.BeforeChange?.ID ?? this.AfterChange?.ID;

        public string MAName => this.BeforeChange?.MAName ?? this.AfterChange?.MAName;

        public ExportFlowRules ExportFlowRules => this.GetObject<ExportFlowRules>("export-flow-rules/export-attribute-flow");

        public ConnectorDeprovision DeprovisionDetails => this.GetObject<ConnectorDeprovision>("de-provisioning-rules/cs-deprovisioning");

        public ExportChange AfterChange => this.GetObject<ExportChange>("export-after-change");

        public FilterRules FilterRules => this.GetObject<FilterRules>("stay-disconnector-rules/stay-disconnector");

        public Error Error => this.GetObject<Error>("error");
    }
}
