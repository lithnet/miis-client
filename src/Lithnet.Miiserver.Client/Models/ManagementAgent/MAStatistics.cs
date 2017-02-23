using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class MAStatistics : XmlObjectBase
    {
        internal MAStatistics(XmlNode node)
            :base(node)
        {
        }

        public int Total => this.GetValue<int>("all");

        public int TotalConnectors => this.GetValue<int>("total-connector");

        public int TotalDisconnectors => this.GetValue<int>("total-disconnector");

        public int Placeholders => this.GetValue<int>("placeholder");

        public int NormalConnectors => this.GetValue<int>("connector");

        public int ExplicitConnectors => this.GetValue<int>("explicit-connector");

        public int NormalDisconnectors => this.GetValue<int>("disconnector");

        public int ExplicitDisconnectors => this.GetValue<int>("explicit-disconnector");

        public int FilteredDisconnectors => this.GetValue<int>("filtered-disconnector");

        public int ImportAdds => this.GetValue<int>("import-add");

        public int ImportUpdates => this.GetValue<int>("import-update");

        public int ImportDeletes => this.GetValue<int>("import-delete");

        public int ImportUnchanged => this.GetValue<int>("import-none");

        public int PendingImportTotal => this.ImportAdds + this.ImportDeletes + this.ImportUpdates;

        public int ExportAdds => this.GetValue<int>("export-add");

        public int ExportUpdates => this.GetValue<int>("export-update");

        public int ExportDeletes => this.GetValue<int>("export-delete");

        public int PendingExportTotal => this.ExportAdds + this.ExportDeletes + this.ExportUpdates;
    }
}
