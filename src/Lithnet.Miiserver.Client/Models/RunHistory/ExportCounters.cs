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

        public int ExportAdd => this.ExportAddDetail.Count;

        public CounterDetail ExportAddDetail => this.GetObject<CounterDetail>("export-add");

        public int ExportUpdate => this.ExportUpdateDetail.Count;

        public CounterDetail ExportUpdateDetail => this.GetObject<CounterDetail>("export-update");

        public int ExportRename => this.ExportRenameDetail.Count;

        public CounterDetail ExportRenameDetail => this.GetObject<CounterDetail>("export-rename");

        public int ExportDelete => this.ExportDeleteDetail.Count;

        public CounterDetail ExportDeleteDetail => this.GetObject<CounterDetail>("export-delete");

        public int ExportDeleteAdd => this.ExportDeleteAddDetail.Count;

        public CounterDetail ExportDeleteAddDetail => this.GetObject<CounterDetail>("export-delete-add");

        public int ExportFailure => this.ExportFailureDetail.Count;

        public CounterDetail ExportFailureDetail => this.GetObject<CounterDetail>("export-failure");

        public bool HasChanges => this.ExportChanges > 0;

        public int ExportChanges => this.ExportAdd + this.ExportUpdate + this.ExportRename + this.ExportDelete + this.ExportDeleteAdd + this.ExportFailure;
    }
}
