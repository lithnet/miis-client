using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class MAStatistics : NodeCache
    {
        internal MAStatistics(XmlNode node)
            :base(node)
        {
        }

        public int Total
        {
            get
            {
                return this.GetValue<int>("all");
            }
        }

        public int TotalConnectors
        {
            get
            {
                return this.GetValue<int>("total-connector");
            }
        }

        public int TotalDisconnectors
        {
            get
            {
                return this.GetValue<int>("total-disconnector");
            }
        }

        public int Placeholders
        {
            get
            {
                return this.GetValue<int>("placeholder");
            }
        }

        public int NormalConnectors
        {
            get
            {
                return this.GetValue<int>("connector");
            }
        }
        
        public int ExplicitConnectors
        {
            get
            {
                return this.GetValue<int>("explicit-connector");
            }
        }

        public int NormalDisconnectors
        {
            get
            {
                return this.GetValue<int>("disconnector");
            }
        }

        public int ExplicitDisconnectors
        {
            get
            {
                return this.GetValue<int>("explicit-disconnector");
            }
        }

        public int FilteredDisconnectors
        {
            get
            {
                return this.GetValue<int>("filtered-disconnector");
            }
        }

        public int ImportAdds
        {
            get
            {
                return this.GetValue<int>("import-add");
            }
        }

        public int ImportUpdates
        {
            get
            {
                return this.GetValue<int>("import-update");
            }
        }

        public int ImportDeletes
        {
            get
            {
                return this.GetValue<int>("import-delete");
            }
        }

        public int ImportUnchanged
        {
            get
            {
                return this.GetValue<int>("import-none");
            }
        }

        public int PendingImportTotal
        {
            get
            {
                return this.ImportAdds + this.ImportDeletes + this.ImportUpdates;
            }
        }

        public int ExportAdds
        {
            get
            {
                return this.GetValue<int>("export-add");
            }
        }

        public int ExportUpdates
        {
            get
            {
                return this.GetValue<int>("export-update");
            }
        }

        public int ExportDeletes
        {
            get
            {
                return this.GetValue<int>("export-delete");
            }
        }

        public int PendingExportTotal
        {
            get
            {
                return this.ExportAdds + this.ExportDeletes + this.ExportUpdates;
            }
        }
    }
}
