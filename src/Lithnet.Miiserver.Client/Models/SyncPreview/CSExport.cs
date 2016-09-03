using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class CSExport : XmlObjectBase
    {
        internal CSExport(XmlNode node)
            : base(node)
        {
        }

        public ExportChange BeforeChange
        {
            get
            {
                return this.GetObject<ExportChange>("export-before-change");
            }
        }

        public Guid? MAID
        {
            get
            {
                return this.BeforeChange?.MAID ?? this.AfterChange?.MAID;
            }
        }

        public Guid? ID
        {
            get
            {
                return this.BeforeChange?.ID ?? this.AfterChange?.ID;
            }
        }

        public string MAName
        {
            get
            {
                return this.BeforeChange?.MAName ?? this.AfterChange?.MAName;
            }
        }

        public ExportFlowRules ExportFlowRules
        {
            get
            {
                return this.GetObject<ExportFlowRules>("export-flow-rules/export-attribute-flow");
            }
        }

        public ConnectorDeprovision DeprovisionDetails
        {
            get
            {
                return this.GetObject<ConnectorDeprovision>("de-provisioning-rules/cs-deprovisioning");
            }
        }

        public ExportChange AfterChange
        {
            get
            {
                return this.GetObject<ExportChange>("export-after-change");
            }
        }

        public FilterRules FilterRules
        {
            get
            {
                return this.GetObject<FilterRules>("stay-disconnector-rules/stay-disconnector");
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
