using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.DirectoryServices.MetadirectoryServices.UI.WebServices;

namespace Lithnet.Miiserver.Client
{
    public class ExportCounters : XmlObjectBase
    {
        private Guid stepID;

        internal ExportCounters(XmlNode node, Guid stepID)
            : base(node)
        {
            this.stepID = stepID;
        }

        public int ExportAdd => this.ExportAddDetail?.Count ?? 0;

        public CounterDetail ExportAddDetail => this.GetObject<CounterDetail>("export-add", this.stepID);

        public int ExportUpdate => this.ExportUpdateDetail?.Count ?? 0;

        public CounterDetail ExportUpdateDetail => this.GetObject<CounterDetail>("export-update", this.stepID);

        public int ExportRename => this.ExportRenameDetail?.Count ?? 0;

        public CounterDetail ExportRenameDetail => this.GetObject<CounterDetail>("export-rename", this.stepID);

        public int ExportDelete => this.ExportDeleteDetail?.Count ?? 0;

        public CounterDetail ExportDeleteDetail => this.GetObject<CounterDetail>("export-delete", this.stepID);

        public int ExportDeleteAdd => this.ExportDeleteAddDetail?.Count ?? 0;

        public CounterDetail ExportDeleteAddDetail => this.GetObject<CounterDetail>("export-delete-add", this.stepID);

        public int ExportFailure => this.ExportFailureDetail?.Count ?? 0;

        public CounterDetail ExportFailureDetail => this.GetObject<CounterDetail>("export-failure", this.stepID);

        public bool HasChanges => this.ExportChanges > 0;

        public int ExportChanges => this.ExportAdd + this.ExportUpdate + this.ExportRename + this.ExportDelete + this.ExportDeleteAdd + this.ExportFailure;
    }
}
