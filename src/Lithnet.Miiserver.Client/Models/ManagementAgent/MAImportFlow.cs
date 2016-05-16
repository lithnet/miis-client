using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class MAImportFlow : ImportFlow
    {
        internal MAImportFlow(XmlNode node, string mvObjectType, string mvattributeName)
            : base(node)
        {
            this.MVAttributeName = mvattributeName;
            this.MVObjectType = mvObjectType;
        }

        public string MVObjectType { get; private set; }

        public string MVAttributeName { get; private set; }
    }
}