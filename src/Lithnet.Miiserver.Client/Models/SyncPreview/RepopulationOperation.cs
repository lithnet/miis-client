using System.Collections.Generic;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class RepopulationOperation : XmlObjectBase
    {
        internal RepopulationOperation(XmlNode node)
            :base(node)
        {
        }

        public string DeletedAttribute => this.GetValue<string>("deleting-attribute/@mv-attribute");

        public IReadOnlyList<RecallImportFlow> ImportFlows => this.GetReadOnlyObjectList<RecallImportFlow>("import-flow");


        public override string ToString()
        {
            return this.DeletedAttribute;
        }
    }
}
