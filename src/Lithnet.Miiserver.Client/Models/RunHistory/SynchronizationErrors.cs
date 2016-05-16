namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Xml;
    using System.Collections.Generic;

    public partial class SynchronizationErrors : NodeCache
    {
        internal SynchronizationErrors(XmlNode node)
            : base(node)
        {
        }

        public IReadOnlyList<ImportError> ImportErrors
        {
            get
            {
                return this.GetReadOnlyObjectList<ImportError>("import-error");
            }
        }

        public IReadOnlyList<ExportError> ExportErrors
        {
            get
            {
                return this.GetReadOnlyObjectList<ExportError>("export-error");
            }
        }
    }
}
