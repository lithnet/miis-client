using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Diagnostics;

namespace Lithnet.Miiserver.Client
{
    [Serializable]
    //batch
    public class CSObjectSearchResultBatch : XmlObjectBase
    {
        internal CSObjectSearchResultBatch(XmlNode node)
            :base(node)
        {
        }

        public int Count
        {
            get
            {
                return this.GetValue<int>("count");
            }
        }

        public int ErrorCount
        {
            get
            {
                return this.GetValue<int>("errorcount");
            }
        }

        public IReadOnlyList<CSObject> CSObjects
        {
            get
            {
                return this.GetReadOnlyObjectList<CSObject>("cs-objects/cs-object");
            }
        }
    }
}
