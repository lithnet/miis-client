using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class ImportFlowGroup : NodeCache
    {
        internal ImportFlowGroup(XmlNode node)
            : base(node)
        {
        }

        public string MVAttribute
        {
            get
            {
                return this.GetValue<string>("@mv-attribute");
            }
        }

        public string Type
        {
            get
            {
                return this.GetValue<string>("@type");
            }
        }
        public IReadOnlyList<ImportFlow> ImportFlows
        {
            get
            {
                return this.GetReadOnlyObjectList<ImportFlow>("import-flow");
            }
        }

        public override string ToString()
        {
            return this.MVAttribute;
        }
    }
}

/*
      <import-flows mv-attribute="ObjectID" type="ranked">
        <import-flow src-ma="{5A4F1F37-94C7-489E-80FC-A11EE901BB53}" cd-object-type="group" id="{EDFFFA0C-9CFC-4037-8313-F705987DA767}">
          <direct-mapping>
            <src-attribute>ObjectID</src-attribute>
          </direct-mapping>
        </import-flow>
        <import-flow src-ma="{5A4F1F37-94C7-489E-80FC-A11EE901BB53}" cd-object-type="shadowOrgUnitGroup" id="{60680A37-B0E8-49E3-A931-84955D1D7BCF}">
          <direct-mapping>
            <src-attribute>ObjectID</src-attribute>
          </direct-mapping>
        </import-flow>
      </import-flows>
*/
