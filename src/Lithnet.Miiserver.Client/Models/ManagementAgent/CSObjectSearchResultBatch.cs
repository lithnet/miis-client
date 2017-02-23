using System;
using System.Collections.Generic;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    [Serializable]
    public class CSObjectSearchResultBatch : XmlObjectBase
    {
        internal CSObjectSearchResultBatch(XmlNode node)
            :base(node)
        {
        }

        public int Count => this.GetValue<int>("count");

        public int ErrorCount => this.GetValue<int>("errorcount");

        public IReadOnlyList<CSObject> CSObjects => this.GetReadOnlyObjectList<CSObject>("cs-objects/cs-object");
    }
}
