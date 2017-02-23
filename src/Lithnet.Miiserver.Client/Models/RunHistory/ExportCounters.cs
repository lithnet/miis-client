using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class ExportCounters : XmlObjectBase
    {
        internal ExportCounters(XmlNode node)
            : base(node)
        {
        }

        public int ExportAdd => this.GetValue<int>("export-add");

        public int ExportUpdate => this.GetValue<int>("export-update");

        public int ExportRename => this.GetValue<int>("export-rename");

        public int ExportDelete => this.GetValue<int>("export-delete");

        public int ExportDeleteAdd => this.GetValue<int>("export-delete-add");

        public int ExportFailure => this.GetValue<int>("export-failure");

        public bool HasChanges => this.ExportChanges > 0;

        public int ExportChanges => this.ExportAdd + this.ExportUpdate + this.ExportRename + this.ExportDelete + this.ExportDeleteAdd + this.ExportFailure;
    }
}
