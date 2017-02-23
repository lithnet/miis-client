using System;
using System.Collections.Generic;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class ImportFlowSet : XmlObjectBase
    {
        internal ImportFlowSet(XmlNode node)
            : base(node)
        {
        }

        public string MVObjectType => this.GetValue<string>("@mv-object-type");

        public IReadOnlyDictionary<string, ImportFlowGroup> ImportFlows
        {
            get
            {
                return this.GetReadOnlyObjectDictionary<string, ImportFlowGroup>("import-flows", t => t.MVAttribute, StringComparer.OrdinalIgnoreCase);
            }
        }

        public override string ToString()
        {
            return this.MVObjectType;
        }
    }
}

/*
 <import-flow-set mv-object-type="group">
    <import-flows mv-attribute="connectedToMonashGoogleApps" type="ranked">
*/
