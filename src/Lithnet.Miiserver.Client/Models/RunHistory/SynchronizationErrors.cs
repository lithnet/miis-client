using System.Collections.Generic;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class SynchronizationErrors : XmlObjectBase
    {
        internal SynchronizationErrors(XmlNode node)
            : base(node)
        {
        }

        public IReadOnlyList<ImportError> ImportErrors => this.GetReadOnlyObjectList<ImportError>("import-error");

        public IReadOnlyList<ExportError> ExportErrors => this.GetReadOnlyObjectList<ExportError>("export-error");
    }
}
