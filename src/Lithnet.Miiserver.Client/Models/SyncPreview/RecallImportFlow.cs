using System;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class RecallImportFlow : ImportFlowResult
    {
        internal RecallImportFlow(XmlNode node)
            :base(node)
        {
        }

        public Guid  SourceMAID => this.GetValue<Guid >("@src-ma-id");


        public Guid SourceObjectID => this.GetValue<Guid>("@src-object-id");

        public override string ToString()
        {
            return this.TargetAttribute;
        }
    }
}
