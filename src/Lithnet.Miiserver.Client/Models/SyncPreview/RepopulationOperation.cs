using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class RepopulationOperation : XmlObjectBase
    {
        internal RepopulationOperation(XmlNode node)
            :base(node)
        {
        }

        public string DeletedAttribute
        {
            get
            {
                return this.GetValue<string>("deleting-attribute/@mv-attribute");
            }
        }

        public IReadOnlyList<RecallImportFlow> ImportFlows
        {
            get
            {
                return this.GetReadOnlyObjectList<RecallImportFlow>("import-flow");
            }
        }

     
        public override string ToString()
        {
            return this.DeletedAttribute;
        }
    }
}
