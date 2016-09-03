namespace Lithnet.Miiserver.Client
{
    using System.Xml;

    public class ExportCounters : XmlObjectBase
    {
        internal ExportCounters(XmlNode node)
            : base(node)
        {
        }

        public int ExportAdd
        {
            get
            {
                return this.GetValue<int>("export-add");
            }
        }

        public int ExportUpdate
        {
            get
            {
                return this.GetValue<int>("export-update");
            }
        }

        public int ExportRename
        {
            get
            {
                return this.GetValue<int>("export-rename");
            }
        }

        public int ExportDelete
        {
            get
            {
                return this.GetValue<int>("export-delete");
            }
        }

        public int ExportDeleteAdd
        {
            get
            {
                return this.GetValue<int>("export-delete-add");
            }
        }

        public int ExportFailure
        {
            get
            {
                return this.GetValue<int>("export-failure");
            }
        }

        public bool HasChanges
        {
            get
            {
                return this.ExportChanges > 0;
            }
        }

        public int ExportChanges
        {
            get
            {
                return this.ExportAdd + this.ExportUpdate + this.ExportRename + this.ExportDelete + this.ExportDeleteAdd + this.ExportFailure;
            }
        }
    }
}
