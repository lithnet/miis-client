using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class RecallImportFlow : ImportFlowResult
    {
        internal RecallImportFlow(XmlNode node)
            :base(node)
        {
        }

        public Guid  SourceMAID
        {
            get
            {
                return this.GetValue<Guid >("@src-ma-id");
            }
        }


        public Guid SourceObjectID
        {
            get
            {
                return this.GetValue<Guid>("@src-object-id");
            }
        }      

        public override string ToString()
        {
            return this.TargetAttribute;
        }
    }
}
